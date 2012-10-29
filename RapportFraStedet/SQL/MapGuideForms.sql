USE [DatabaseForms]
GO
/****** Object:  Table [dbo].[Applications]    Script Date: 29-10-2012 14:13:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Applications](
	[ApplicationName] [nvarchar](235) NOT NULL,
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](256) NULL,
 CONSTRAINT [Applications_PK__Applications__0000000000000441] PRIMARY KEY CLUSTERED 
(
	[ApplicationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Company]    Script Date: 29-10-2012 14:13:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Company](
	[CompanyId] [int] IDENTITY(5,1) NOT NULL,
	[Name] [nvarchar](500) NULL,
 CONSTRAINT [Company_PK_Company] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Field]    Script Date: 29-10-2012 14:13:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Field](
	[FieldId] [int] IDENTITY(68,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[FormId] [int] NOT NULL,
	[FieldTypeId] [int] NOT NULL,
	[FieldOrder] [int] NOT NULL,
	[Required] [bit] NOT NULL,
	[FieldColumn] [nvarchar](100) NULL,
	[Data] [nvarchar](100) NULL,
 CONSTRAINT [Field_PK_Field] PRIMARY KEY CLUSTERED 
(
	[FieldId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FieldType]    Script Date: 29-10-2012 14:13:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FieldType](
	[FieldTypeId] [int] NOT NULL,
	[Name] [nvarchar](100) NULL,
 CONSTRAINT [FieldType_PK_FieldType] PRIMARY KEY CLUSTERED 
(
	[FieldTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Form]    Script Date: 29-10-2012 14:13:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Form](
	[FormId] [int] IDENTITY(12,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Email] [nvarchar](100) NULL,
	[ResourceName] [nvarchar](500) NULL,
	[ClassName] [nvarchar](100) NULL,
	[UploadPhysicalPath] [nvarchar](500) NULL,
	[UploadVirtualPath] [nvarchar](500) NULL,
	[ViewEmailToSenderOnCreate] [nvarchar](100) NULL,
	[ViewEmailToReceiverOnCreate] [nvarchar](100) NULL,
	[ViewEmailToSenderOnEdit] [nvarchar](100) NULL,
	[ViewEmailToReceiverOnEdit] [nvarchar](100) NULL,
	[ViewEmailToSenderOnDelete] [nvarchar](100) NULL,
	[ViewEmailToReceiverOnDelete] [nvarchar](100) NULL,
	[SRS] [nvarchar](100) NULL,
 CONSTRAINT [Form_PK_Form] PRIMARY KEY CLUSTERED 
(
	[FormId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FormGroup]    Script Date: 29-10-2012 14:13:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FormGroup](
	[GroupId] [int] NOT NULL,
	[FormId] [int] NOT NULL,
 CONSTRAINT [FormGroup_PK_FormGroup] PRIMARY KEY CLUSTERED 
(
	[GroupId] ASC,
	[FormId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Group]    Script Date: 29-10-2012 14:13:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Group](
	[GroupId] [int] IDENTITY(16,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[DefaultFormId] [int] NULL,
	[Role] [nvarchar](100) NULL,
 CONSTRAINT [Group_PK_Group] PRIMARY KEY CLUSTERED 
(
	[GroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Memberships]    Script Date: 29-10-2012 14:13:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Memberships](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Password] [nvarchar](128) NOT NULL,
	[PasswordFormat] [int] NOT NULL,
	[PasswordSalt] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[PasswordQuestion] [nvarchar](256) NULL,
	[PasswordAnswer] [nvarchar](128) NULL,
	[IsApproved] [bit] NOT NULL,
	[IsLockedOut] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[LastLoginDate] [datetime] NOT NULL,
	[LastPasswordChangedDate] [datetime] NOT NULL,
	[LastLockoutDate] [datetime] NOT NULL,
	[FailedPasswordAttemptCount] [int] NOT NULL,
	[FailedPasswordAttemptWindowStart] [datetime] NOT NULL,
	[FailedPasswordAnswerAttemptCount] [int] NOT NULL,
	[FailedPasswordAnswerAttemptWindowsStart] [datetime] NOT NULL,
	[Comment] [nvarchar](256) NULL,
 CONSTRAINT [Memberships_PK__Memberships__000000000000046D] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Permission]    Script Date: 29-10-2012 14:13:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permission](
	[PermissionId] [int] IDENTITY(42,1) NOT NULL,
	[FieldId] [int] NOT NULL,
	[ViewId] [int] NOT NULL,
	[PermissionTypeId] [int] NOT NULL,
 CONSTRAINT [Permission_PK_Permission] PRIMARY KEY CLUSTERED 
(
	[PermissionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PermissionType]    Script Date: 29-10-2012 14:13:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PermissionType](
	[PermissionTypeId] [int] NOT NULL,
	[Name] [nvarchar](100) NULL,
 CONSTRAINT [PermissionType_PK__PermissionType__0000000000000300] PRIMARY KEY CLUSTERED 
(
	[PermissionTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [PermissionType_UQ__PermissionType__00000000000002F2] UNIQUE NONCLUSTERED 
(
	[PermissionTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Profiles]    Script Date: 29-10-2012 14:13:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Profiles](
	[UserId] [uniqueidentifier] NOT NULL,
	[PropertyNames] [nvarchar](4000) NOT NULL,
	[PropertyValueStrings] [nvarchar](4000) NOT NULL,
	[PropertyValueBinary] [image] NOT NULL,
	[LastUpdatedDate] [datetime] NOT NULL,
 CONSTRAINT [Profiles_PK__Profiles__000000000000047D] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Roles]    Script Date: 29-10-2012 14:13:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
	[RoleName] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](256) NULL,
 CONSTRAINT [Roles_PK__Roles__000000000000048B] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Selection]    Script Date: 29-10-2012 14:13:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Selection](
	[SelectionId] [int] IDENTITY(52,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Email] [nvarchar](100) NULL,
	[FieldId] [int] NOT NULL,
	[SelectionOrder] [int] NULL,
 CONSTRAINT [Selection_PK_Selection] PRIMARY KEY CLUSTERED 
(
	[SelectionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 29-10-2012 14:13:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[IsAnonymous] [bit] NOT NULL,
	[LastActivityDate] [datetime] NOT NULL,
 CONSTRAINT [Users_PK__Users__000000000000049B] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UsersInRoles]    Script Date: 29-10-2012 14:13:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsersInRoles](
	[UserId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [UsersInRoles_PK__UsersInRoles__00000000000004A5] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[View]    Script Date: 29-10-2012 14:13:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[View](
	[ViewId] [int] IDENTITY(41,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[GroupId] [int] NOT NULL,
	[ViewTypeId] [int] NOT NULL,
 CONSTRAINT [View_PK_View] PRIMARY KEY CLUSTERED 
(
	[ViewId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ViewType]    Script Date: 29-10-2012 14:13:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ViewType](
	[ViewTypeId] [int] NOT NULL,
	[Name] [nvarchar](100) NULL,
 CONSTRAINT [ViewType_PK_ViewType] PRIMARY KEY CLUSTERED 
(
	[ViewTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Field]  WITH CHECK ADD  CONSTRAINT [Field_FieldType_Field] FOREIGN KEY([FieldTypeId])
REFERENCES [dbo].[FieldType] ([FieldTypeId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Field] CHECK CONSTRAINT [Field_FieldType_Field]
GO
ALTER TABLE [dbo].[Field]  WITH CHECK ADD  CONSTRAINT [Field_Form_Field] FOREIGN KEY([FormId])
REFERENCES [dbo].[Form] ([FormId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Field] CHECK CONSTRAINT [Field_Form_Field]
GO
ALTER TABLE [dbo].[FormGroup]  WITH CHECK ADD  CONSTRAINT [FormGroup_Form_FormGroup] FOREIGN KEY([FormId])
REFERENCES [dbo].[Form] ([FormId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[FormGroup] CHECK CONSTRAINT [FormGroup_Form_FormGroup]
GO
ALTER TABLE [dbo].[FormGroup]  WITH CHECK ADD  CONSTRAINT [FormGroup_Group_FormGroup] FOREIGN KEY([GroupId])
REFERENCES [dbo].[Group] ([GroupId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[FormGroup] CHECK CONSTRAINT [FormGroup_Group_FormGroup]
GO
ALTER TABLE [dbo].[Memberships]  WITH CHECK ADD  CONSTRAINT [Memberships_MembershipApplication] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[Applications] ([ApplicationId])
GO
ALTER TABLE [dbo].[Memberships] CHECK CONSTRAINT [Memberships_MembershipApplication]
GO
ALTER TABLE [dbo].[Memberships]  WITH CHECK ADD  CONSTRAINT [Memberships_MembershipUser] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Memberships] CHECK CONSTRAINT [Memberships_MembershipUser]
GO
ALTER TABLE [dbo].[Permission]  WITH CHECK ADD  CONSTRAINT [Permission_Field_Permission] FOREIGN KEY([FieldId])
REFERENCES [dbo].[Field] ([FieldId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Permission] CHECK CONSTRAINT [Permission_Field_Permission]
GO
ALTER TABLE [dbo].[Permission]  WITH CHECK ADD  CONSTRAINT [Permission_Permission_PermissionType] FOREIGN KEY([PermissionTypeId])
REFERENCES [dbo].[PermissionType] ([PermissionTypeId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Permission] CHECK CONSTRAINT [Permission_Permission_PermissionType]
GO
ALTER TABLE [dbo].[Permission]  WITH CHECK ADD  CONSTRAINT [Permission_View_Permission] FOREIGN KEY([ViewId])
REFERENCES [dbo].[View] ([ViewId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Permission] CHECK CONSTRAINT [Permission_View_Permission]
GO
ALTER TABLE [dbo].[Profiles]  WITH CHECK ADD  CONSTRAINT [Profiles_UserProfile] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Profiles] CHECK CONSTRAINT [Profiles_UserProfile]
GO
ALTER TABLE [dbo].[Roles]  WITH CHECK ADD  CONSTRAINT [Roles_RoleApplication] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[Applications] ([ApplicationId])
GO
ALTER TABLE [dbo].[Roles] CHECK CONSTRAINT [Roles_RoleApplication]
GO
ALTER TABLE [dbo].[Selection]  WITH CHECK ADD  CONSTRAINT [Selection_Field_Selection] FOREIGN KEY([FieldId])
REFERENCES [dbo].[Field] ([FieldId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Selection] CHECK CONSTRAINT [Selection_Field_Selection]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [Users_UserApplication] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[Applications] ([ApplicationId])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [Users_UserApplication]
GO
ALTER TABLE [dbo].[UsersInRoles]  WITH CHECK ADD  CONSTRAINT [UsersInRoles_UsersInRoleRole] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
GO
ALTER TABLE [dbo].[UsersInRoles] CHECK CONSTRAINT [UsersInRoles_UsersInRoleRole]
GO
ALTER TABLE [dbo].[UsersInRoles]  WITH CHECK ADD  CONSTRAINT [UsersInRoles_UsersInRoleUser] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[UsersInRoles] CHECK CONSTRAINT [UsersInRoles_UsersInRoleUser]
GO
ALTER TABLE [dbo].[View]  WITH CHECK ADD  CONSTRAINT [View_Group_View] FOREIGN KEY([GroupId])
REFERENCES [dbo].[Group] ([GroupId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[View] CHECK CONSTRAINT [View_Group_View]
GO
ALTER TABLE [dbo].[View]  WITH CHECK ADD  CONSTRAINT [View_ViewType_View] FOREIGN KEY([ViewTypeId])
REFERENCES [dbo].[ViewType] ([ViewTypeId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[View] CHECK CONSTRAINT [View_ViewType_View]
GO
