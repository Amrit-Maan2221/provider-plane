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
