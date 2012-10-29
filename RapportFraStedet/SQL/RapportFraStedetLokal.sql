USE [RapportFraStedetLokal]
GO
/****** Object:  StoredProcedure [dbo].[HentTemaer]    Script Date: 29-10-2012 14:12:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[HentTemaer] 
	-- Add the parameters for the stored procedure here
	@nr int,
	@X nvarchar(50), 
	@Y nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
	SELECT Id, Navn, Beskrivelse, Logo, MapAgent, ApplicationDefinition, Besked, dbo.ModtagerIndberetning(Id,@X, @Y) AS ModtagerIndberetning from Tema where KommuneNr=@nr and synlig=1 order by Navn;
END

GO
/****** Object:  UserDefinedFunction [dbo].[CheckIndberetning]    Script Date: 29-10-2012 14:12:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[CheckIndberetning]
(
	-- Add the parameters for the function here
	@temaId int,
	@geometri nvarchar(MAX)
)
RETURNS bit
AS
BEGIN
	-- Declare the return variable here
	DECLARE @ResultVar bit
	DECLARE @a bit
	DECLARE @b bit

	-- Add the T-SQL statements to compute the return value here

	BEGIN
		SELECT @a = COUNT(*) from temapolygon a inner join polygon b on (a.PolygonId=b.Id) where a.ModtagerIndberetning=0 and b.Geometri.STIntersects(geometry::STGeomFromText(@geometri, 25832))='True' and a.TemaId=@temaId;
		SELECT @b = COUNT(*) from temapolygon a inner join polygon b on (a.PolygonId=b.Id) where a.ModtagerIndberetning=1 and b.Geometri.STIntersects(geometry::STGeomFromText(@geometri, 25832))='True' and a.TemaId=@temaId;
	END
	SELECT @ResultVar = case when @a=0 and @b>0 then 1 else 0 end;
	-- Return the result of the function
	RETURN @ResultVar

END

GO
/****** Object:  UserDefinedFunction [dbo].[ModtagerIndberetning]    Script Date: 29-10-2012 14:12:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[ModtagerIndberetning]
(
	-- Add the parameters for the function here
	@temaId int,
	@X nvarchar(50), 
	@Y nvarchar(50)

)
RETURNS bit
AS
BEGIN
	-- Declare the return variable here
	DECLARE @ResultVar bit
	DECLARE @a bit
	DECLARE @b bit

	-- Add the T-SQL statements to compute the return value here
	IF(@X='0' or @Y='0')
	BEGIN
		SELECT @a = COUNT(*) from temapolygon a inner join polygon b on (a.PolygonId=b.Id) where a.ModtagerIndberetning=0 and a.TemaId=@temaId;
		SELECT @b = COUNT(*) from temapolygon a inner join polygon b on (a.PolygonId=b.Id) where a.ModtagerIndberetning=1 and a.TemaId=@temaId;
	END
	ELSE
	BEGIN
		SELECT @a = COUNT(*) from temapolygon a inner join polygon b on (a.PolygonId=b.Id) where a.ModtagerIndberetning=0 and b.Geometri.STIntersects(geometry::STGeomFromText('POINT ('+@X+' '+@Y+')', 25832))='True' and a.TemaId=@temaId;
		SELECT @b = COUNT(*) from temapolygon a inner join polygon b on (a.PolygonId=b.Id) where a.ModtagerIndberetning=1 and b.Geometri.STIntersects(geometry::STGeomFromText('POINT ('+@X+' '+@Y+')', 25832))='True' and a.TemaId=@temaId;
	END
	SELECT @ResultVar = case when @a=0 and @b>0 then 1 else 0 end;
	-- Return the result of the function
	RETURN @ResultVar

END

GO
/****** Object:  Table [dbo].[Polygon]    Script Date: 29-10-2012 14:12:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Polygon](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UniqueId] [nvarchar](50) NULL,
	[UserId] [nvarchar](50) NULL,
	[Navn] [nvarchar](50) NULL,
	[URL] [nvarchar](500) NULL,
	[Email] [nvarchar](500) NULL,
	[Geometri] [geometry] NULL,
 CONSTRAINT [PK_Kommuner] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tema]    Script Date: 29-10-2012 14:12:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tema](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Navn] [nvarchar](50) NULL,
	[Logo] [nvarchar](500) NULL,
	[Beskrivelse] [nvarchar](500) NULL,
	[MapAgent] [nvarchar](500) NULL,
	[KommuneNr] [int] NULL,
	[Besked] [nvarchar](500) NULL,
	[Gennemtving] [bit] NULL,
	[ApplicationDefinition] [nvarchar](500) NULL,
	[Synlig] [bit] NULL,
 CONSTRAINT [PK_Tema] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TemaPolygon]    Script Date: 29-10-2012 14:12:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TemaPolygon](
	[TemaId] [int] NOT NULL,
	[PolygonId] [int] NOT NULL,
	[ModtagerIndberetning] [bit] NULL,
 CONSTRAINT [PK_TemaPolygon] PRIMARY KEY CLUSTERED 
(
	[TemaId] ASC,
	[PolygonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[TemaPolygon]  WITH CHECK ADD  CONSTRAINT [FK_TemaPolygon_Polygon] FOREIGN KEY([PolygonId])
REFERENCES [dbo].[Polygon] ([Id])
GO
ALTER TABLE [dbo].[TemaPolygon] CHECK CONSTRAINT [FK_TemaPolygon_Polygon]
GO
ALTER TABLE [dbo].[TemaPolygon]  WITH CHECK ADD  CONSTRAINT [FK_TemaPolygon_Tema] FOREIGN KEY([TemaId])
REFERENCES [dbo].[Tema] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TemaPolygon] CHECK CONSTRAINT [FK_TemaPolygon_Tema]
GO
