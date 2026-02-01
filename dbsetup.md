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


# Databases Db

```sql
CREATE Database DatabasesDb
Use DatabasesDb

CREATE TABLE DatabasesTbl (
    idDatabaseId INT IDENTITY PRIMARY KEY,
    vcDatabaseName VARCHAR(128) NOT NULL,
    vcServerName   VARCHAR(128) NOT NULL,
    vcDbUser       VARCHAR(128) NOT NULL,
    vcDbPassword  VARCHAR(256) NOT NULL, -- or KeyVault ref
    vcRegion      VARCHAR(32) NOT NULL,  -- ca-central-1, eu-west-1
    vcProductCode VARCHAR(50) NOT NULL,  -- WORKSHOP
    bitActive BIT NOT NULL DEFAULT 1,
    intCreatedBy INT NOT NULL,
    intUpdatedBy INT NOT NULL,
    dtCreated DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    dtUpdated DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
);
```

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
    bitActive       BIT NOT NULL DEFAULT 1,
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


CREATE TABLE SubscriptionPlans (
    idSubscriptionPlanId INT IDENTITY(1,1) PRIMARY KEY,

    vcPlanCode VARCHAR(50) NOT NULL,
    vcPlanName VARCHAR(150) NOT NULL,

    vcDescription VARCHAR(255) NULL,

    fltMonthlyPrice DECIMAL(10,2) NOT NULL,
    fltYearlyPrice DECIMAL(10,2) NULL,

    bitActive BIT NOT NULL DEFAULT 1,
	bitHasAllFeatures BIT NOT NULL,
    intCreatedBy INT NOT NULL,
    intUpdatedBy INT NOT NULL,
    dtCreated DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    dtUpdated DATETIME2 NOT NULL DEFAULT SYSDATETIME()
);

CREATE TABLE SubscriptionPlanFeatures (
    idSubscriptionPlanFeatureId INT IDENTITY(1,1) PRIMARY KEY,

    intSubscriptionPlanId INT NOT NULL,
    intProductFeatureId INT NOT NULL,

    bitIsEnabled BIT NOT NULL DEFAULT 1,

    -- optional future use
    intLimitValue INT NULL,
    vcLimitUnit VARCHAR(50) NULL,

    intCreatedBy INT NOT NULL,
    intUpdatedBy INT NOT NULL,
    dtCreated DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    dtUpdated DATETIME2 NOT NULL DEFAULT SYSDATETIME(),

    CONSTRAINT FK_SPF_Plan
        FOREIGN KEY (intSubscriptionPlanId)
        REFERENCES SubscriptionPlans(idSubscriptionPlanId),

    CONSTRAINT FK_SPF_Feature
        FOREIGN KEY (intProductFeatureId)
        REFERENCES ProductFeatures(idProductFeatureId)
);

CREATE TABLE SubscriptionStatuses (
    intSubscriptionStatusId INT PRIMARY KEY,
    vcSubscriptionStatus    VARCHAR(20) NOT NULL,

    intCreatedBy INT NOT NULL,
    intUpdatedBy INT NOT NULL,

    dtCreated DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    dtUpdated DATETIME2 NOT NULL DEFAULT SYSDATETIME()
);

INSERT INTO SubscriptionStatuses (
    intSubscriptionStatusId,
    vcSubscriptionStatus,
    intCreatedBy,
    intUpdatedBy
)
VALUES
    (1, 'trial',     -1, -1),
    (2, 'active',    -1, -1),
    (3, 'paused',    -1, -1),
    (4, 'cancelled', -1, -1),
    (5, 'expired',   -1, -1);


CREATE TABLE Subscriptions (
    idSubscriptionId INT IDENTITY(1,1) PRIMARY KEY,

    intTenantId INT NOT NULL,
    intSubscriptionPlanId INT NOT NULL,
    intSubscriptionStatusId INT NOT NULL,

    dtStartDate DATETIME2 NOT NULL,
    dtEndDate DATETIME2 NULL,
    dtNextBillingDate DATETIME2 NULL,

    vcExternalReference VARCHAR(100) NULL,

    intCreatedBy INT NOT NULL,
    intUpdatedBy INT NOT NULL,
    dtCreated DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    dtUpdated DATETIME2 NOT NULL DEFAULT SYSDATETIME(),

    CONSTRAINT FK_Subscriptions_Tenant
        FOREIGN KEY (intTenantId)
        REFERENCES Tenants(idTenantId),

    CONSTRAINT FK_Subscriptions_Plan
        FOREIGN KEY (intSubscriptionPlanId)
        REFERENCES SubscriptionPlans(idSubscriptionPlanId),

    CONSTRAINT FK_Subscriptions_Status
        FOREIGN KEY (intSubscriptionStatusId)
        REFERENCES SubscriptionStatuses(intSubscriptionStatusId)
);



```
