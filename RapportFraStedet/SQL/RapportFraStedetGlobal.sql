USE [RapportFraStedetGlobal]
GO
/****** Object:  StoredProcedure [dbo].[HentAlleKommuner]    Script Date: 29-10-2012 14:10:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[HentAlleKommuner] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
	SELECT Navn, Logo, URL, ModtagerIndberetning, Besked, Nr from Kommune where Id<99 order by Navn;
END

GO
/****** Object:  StoredProcedure [dbo].[HentDefaultKommune]    Script Date: 29-10-2012 14:10:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[HentDefaultKommune] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
	SELECT Navn, Logo, URL, ModtagerIndberetning, Besked, Nr from Kommune where Id=99;
END

GO
/****** Object:  StoredProcedure [dbo].[HentKommune]    Script Date: 29-10-2012 14:10:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[HentKommune] 
	-- Add the parameters for the stored procedure here
	@X nvarchar(50), 
	@Y nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
	SELECT Navn, Logo, URL, ModtagerIndberetning, Besked, Nr from Kommune where Geometri.STIntersects(geometry::STGeomFromText('POINT ('+@X+' '+@Y+')', 25832))='True';
END

GO
/****** Object:  Table [dbo].[Kommune]    Script Date: 29-10-2012 14:10:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Kommune](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UniqueId] [nvarchar](50) NULL,
	[UserId] [nvarchar](50) NULL,
	[Navn] [nvarchar](50) NULL,
	[Logo] [nvarchar](500) NULL,
	[ModtagerIndberetning] [bit] NULL,
	[URL] [nvarchar](500) NULL,
	[Besked] [nvarchar](500) NULL,
	[Geometri] [geometry] NULL,
	[Nr] [int] NULL,
	[Dato] [datetime] NULL,
 CONSTRAINT [PK_Kommuner] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
