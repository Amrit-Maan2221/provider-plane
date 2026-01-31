# DBSetup

## TagsDb
```sql
CREATE TABLE [dbo].[TagsGroups](
	[idTagGroupId] [int] IDENTITY(1,1) NOT NULL,
	[vcTagGroupName] [varchar](150) NOT NULL,
	[bitIsActive] [bit] NOT NULL,
	[dtCreated] [datetime2](7) NOT NULL,
	[dtUpdated] [datetime2](7) NOT NULL,
	[intCreatedBy] [int] NOT NULL,
	[intUpdatedBy] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idTagGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TagsGroups] ADD  DEFAULT ((1)) FOR [bitIsActive]
GO

ALTER TABLE [dbo].[TagsGroups] ADD  DEFAULT (sysdatetime()) FOR [dtCreated]
GO

ALTER TABLE [dbo].[TagsGroups] ADD  DEFAULT (sysdatetime()) FOR [dtUpdated]
GO


CREATE TABLE Tags (
    idTagId INT IDENTITY(1,1) PRIMARY KEY,
    intTagGroupId INT NOT NULL,
    vcTagName       VARCHAR(150) NOT NULL,
    vcDescription   VARCHAR(255) NULL,
    dtCreated       DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    dtUpdated       DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    intCreatedBy INT NOT NULL,
    intUpdatedBy INT NOT NULL,
    CONSTRAINT FK_Tags_TagGroups
        FOREIGN KEY (intTagGroupId)
        REFERENCES TagsGroups(idTagGroupId)
);

CREATE UNIQUE INDEX UX_Tags_TagKey
ON Tags(vcTagName);


CREATE TABLE TagAssignments (
    idTagAssignmentId INT IDENTITY(1,1) PRIMARY KEY,
    intTagId INT NOT NULL,
    vcEntityType  VARCHAR(32) NOT NULL,  -- Tenant, Invoice, Subscription
    intEntityId   INT NOT NULL,
    dtCreated       DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    dtUpdated       DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    intCreatedBy INT NOT NULL,
    intUpdatedBy INT NOT NULL,
    CONSTRAINT FK_TagAssignments_Tags
        FOREIGN KEY (intTagId)
        REFERENCES Tags(idTagId)
        ON DELETE CASCADE
);

CREATE UNIQUE INDEX UX_TagAssignments_Unique
ON TagAssignments (intTagId, vcEntityType, intEntityId);
```


# Contacts DB

```sql
USE ContactsDb;

CREATE TABLE Contacts (
    idContactId INT IDENTITY(1,1) PRIMARY KEY,

    vcFirstName VARCHAR(100) NOT NULL,
    vcLastName  VARCHAR(100) NOT NULL,
    vcEmail     VARCHAR(255) NOT NULL,
    vcPhone     VARCHAR(30) NOT NULL,
    bitIsActive BIT NOT NULL DEFAULT 1,
    intCreatedBy INT NOT NULL,
    intUpdatedBy INT NOT NULL,
    dtCreated   DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    dtUpdated   DATETIME2 NOT NULL DEFAULT SYSDATETIME()
);

CREATE UNIQUE INDEX UX_Contacts_Email
ON Contacts(vcEmail);
CREATE UNIQUE INDEX UX_Contacts_Phone
ON Contacts(vcPhone);

CREATE TABLE ContactLinks (
    idContactLinkId INT IDENTITY(1,1) PRIMARY KEY,

    intContactId INT NOT NULL,

    vcEntityType VARCHAR(32) NOT NULL,  -- Tenant, Company, Workshop
    intEntityId  INT NOT NULL,

    vcRole       VARCHAR(32) NOT NULL,   -- Owner, Admin, Billing, Manager
    bitIsPrimary BIT NOT NULL DEFAULT 0,

    
    intCreatedBy INT NOT NULL,
    intUpdatedBy INT NOT NULL,
    dtCreated   DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    dtUpdated   DATETIME2 NOT NULL DEFAULT SYSDATETIME()

    CONSTRAINT FK_ContactLinks_Contacts
        FOREIGN KEY (intContactId)
        REFERENCES Contacts(idContactId)
        ON DELETE CASCADE
);
```


# ProviderPlaneDb


```sql
USE ProviderPlane;


CREATE TABLE [dbo].[Tenants](
	[idTenantId] [int] IDENTITY(1,1) NOT NULL,
	[vcName] [varchar](128) NOT NULL,
	[vcSlug] [varchar](128) NOT NULL,
	[bitActive] [bit] NOT NULL,
	[vcCountry] [varchar](2) NOT NULL,
	[intTimezoneId] [int] NOT NULL,
	[dtCreated] [datetime2](7) NOT NULL,
	[dtUpdated] [datetime2](7) NOT NULL,
	[intCreatedBy] [int] NOT NULL,
	[intUpdatedBy] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idTenantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[vcSlug] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Tenants] ADD  DEFAULT (sysdatetime()) FOR [dtCreated]
GO

ALTER TABLE [dbo].[Tenants] ADD  DEFAULT (sysdatetime()) FOR [dtUpdated]
GO

CREATE TABLE Products (
    idProductId INT IDENTITY(1,1) PRIMARY KEY,
    vcProductName     VARCHAR(64) NOT NULL,
    vcDescription     VARCHAR(255) NULL,
    bitIsActive       BIT NOT NULL DEFAULT 1,
    bitIsPublic       BIT NOT NULL DEFAULT 1,
    intCreatedBy INT NOT NULL,
    intUpdatedBy INT NOT NULL,

    dtCreated    DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    dtUpdated    DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
);


CREATE TABLE ProductFeatures (
    idProductFeatureId INT IDENTITY(1,1) PRIMARY KEY,
    intProductId INT NOT NULL,
    vcFeatureKey   VARCHAR(100) NOT NULL,   -- inspections.create
    vcFeatureName  VARCHAR(150) NOT NULL,
    vcDescription VARCHAR(255) NULL,
    bitIsActive    BIT NOT NULL DEFAULT 1,
    intCreatedBy INT NOT NULL,
    intUpdatedBy INT NOT NULL,
    dtCreated    DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    dtUpdated    DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT FK_ProductFeatures_Products
        FOREIGN KEY (intProductId)
        REFERENCES Products(idProductId),
);


CREATE TABLE EntitlementStatuses(
    intEntitlementStatusId INT  PRIMARY KEY,
    vcEntitlementStatus  VARCHAR(20) NOT NULL,
    intCreatedBy INT NOT NULL,
    intUpdatedBy INT NOT NULL,

    dtCreated DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    dtUpdated DATETIME2 NOT NULL DEFAULT SYSDATETIME()
)


INSERT INTO EntitlementStatuses (
    intEntitlementStatusId,
    vcEntitlementStatus,
    intCreatedBy,
    intUpdatedBy
)
VALUES
    (1, 'active',    -1, -1),
    (2, 'trial',     -1, -1),
    (3, 'suspended', -1, -1),
    (4, 'expired',   -1, -1),
    (5, 'revoked',   -1, -1);


CREATE TABLE TenantProductEntitlements (
    idTenantProductEntitlementId INT IDENTITY(1,1) PRIMARY KEY,

    intTenantId INT NOT NULL,
    intProductId INT NOT NULL,
    intEntitlementStatusId INT NOT NULL,

    dtStartDate DATETIME2 NOT NULL,
    dtEndDate DATETIME2 NULL,

    intCreatedBy INT NOT NULL,
    intUpdatedBy INT NOT NULL,
    dtCreated DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    dtUpdated DATETIME2 NOT NULL DEFAULT SYSDATETIME(),

    CONSTRAINT FK_TPE_Tenants
        FOREIGN KEY (intTenantId) REFERENCES Tenants(idTenantId),

    CONSTRAINT FK_TPE_Products
        FOREIGN KEY (intProductId) REFERENCES Products(idProductId),

    CONSTRAINT FK_TPE_Status
        FOREIGN KEY (intEntitlementStatusId)
        REFERENCES EntitlementStatuses(intEntitlementStatusId)
);


CREATE UNIQUE INDEX UX_TenantProductEntitlements
ON TenantProductEntitlements (intTenantId, intProductId);


CREATE TABLE TenantProductFeatureEntitlements (
    idTenantProductFeatureEntitlementId INT IDENTITY(1,1) PRIMARY KEY,

    intTenantId INT NOT NULL,
    intProductFeatureId INT NOT NULL,
    intEntitlementStatusId INT NOT NULL,

    dtStartDate DATETIME2 NOT NULL,
    dtEndDate DATETIME2 NULL,

    intCreatedBy INT NOT NULL,
    intUpdatedBy INT NOT NULL,
    dtCreated DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    dtUpdated DATETIME2 NOT NULL DEFAULT SYSDATETIME(),

    CONSTRAINT FK_TPFE_Tenants
        FOREIGN KEY (intTenantId) REFERENCES Tenants(idTenantId),

    CONSTRAINT FK_TPFE_Features
        FOREIGN KEY (intProductFeatureId)
        REFERENCES ProductFeatures(idProductFeatureId),

    CONSTRAINT FK_TPFE_Status
        FOREIGN KEY (intEntitlementStatusId)
        REFERENCES EntitlementStatuses(intEntitlementStatusId)
);


CREATE UNIQUE INDEX UX_TenantProductFeatureEntitlements
ON TenantProductFeatureEntitlements (intTenantId, intProductFeatureId);

```
