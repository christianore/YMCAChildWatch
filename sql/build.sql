USE [master]
GO
/****** Object:  Database [ymca_child_watch]    Script Date: 4/18/2018 9:06:33 PM ******/
CREATE DATABASE [ymca_child_watch]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ymca_child_watch', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\ymca_child_watch.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ymca_child_watch_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\ymca_child_watch_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [ymca_child_watch] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ymca_child_watch].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ymca_child_watch] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ymca_child_watch] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ymca_child_watch] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ymca_child_watch] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ymca_child_watch] SET ARITHABORT OFF 
GO
ALTER DATABASE [ymca_child_watch] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [ymca_child_watch] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ymca_child_watch] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ymca_child_watch] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ymca_child_watch] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ymca_child_watch] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ymca_child_watch] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ymca_child_watch] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ymca_child_watch] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ymca_child_watch] SET  ENABLE_BROKER 
GO
ALTER DATABASE [ymca_child_watch] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ymca_child_watch] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ymca_child_watch] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ymca_child_watch] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ymca_child_watch] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ymca_child_watch] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ymca_child_watch] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ymca_child_watch] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ymca_child_watch] SET  MULTI_USER 
GO
ALTER DATABASE [ymca_child_watch] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ymca_child_watch] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ymca_child_watch] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ymca_child_watch] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ymca_child_watch] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ymca_child_watch] SET QUERY_STORE = OFF
GO
USE [ymca_child_watch]
GO
ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [ymca_child_watch]
GO
/****** Object:  User [capstone]    Script Date: 4/18/2018 9:06:33 PM ******/
CREATE USER [capstone] FOR LOGIN [capstone] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [capstone]
GO
ALTER ROLE [db_securityadmin] ADD MEMBER [capstone]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [capstone]
GO
ALTER ROLE [db_datareader] ADD MEMBER [capstone]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [capstone]
GO
/****** Object:  UserDefinedFunction [dbo].[f_signin_open]    Script Date: 4/18/2018 9:06:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================
-- Template generated from Template Explorer using:
-- Create Scalar Function (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 

-- =============================================
-- Author:		Christian Ore
-- Create date: 3/28/2018
-- Description:	Determine if a signin log is open or not.
-- =============================================
CREATE FUNCTION [dbo].[f_signin_open]
(
	@signin_id INT 
)
RETURNS INT
AS
BEGIN
	DECLARE @open BIT = 0
	SELECT
		@open = CASE  WHEN COUNT(signin_id) > 0 THEN 0 ELSE 1 END 
	FROM
		signin_log WITH(NOLOCK)
	WHERE
		signin_id = @signin_id
		AND CAST(time_in AS DATE) = CAST(GETDATE() AS DATE)
		AND time_out IS NULL

	RETURN @open
END
GO
/****** Object:  Table [dbo].[child]    Script Date: 4/18/2018 9:06:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[child](
	[child_id] [int] IDENTITY(1,1) NOT NULL,
	[child_fName] [varchar](50) NOT NULL,
	[child_lName] [varchar](75) NOT NULL,
	[birthdate] [date] NULL,
 CONSTRAINT [child_PK] PRIMARY KEY CLUSTERED 
(
	[child_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[employee]    Script Date: 4/18/2018 9:06:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[employee](
	[emp_id] [int] IDENTITY(1,1) NOT NULL,
	[emp_firstname] [varchar](100) NOT NULL,
	[emp_lastname] [varchar](100) NOT NULL,
	[emp_username] [varbinary](max) NOT NULL,
	[emp_password] [varbinary](max) NOT NULL,
	[emp_admin_flag] [bit] NOT NULL,
	[emp_reset_flag] [bit] NOT NULL,
	[emp_blocked] [bit] NULL,
	[emp_login_attempts] [int] NULL,
 CONSTRAINT [employee_PK] PRIMARY KEY CLUSTERED 
(
	[emp_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[location]    Script Date: 4/18/2018 9:06:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[location](
	[loc_id] [int] IDENTITY(1,1) NOT NULL,
	[loc_name] [varchar](30) NOT NULL,
	[loc_age_min] [smallint] NOT NULL,
	[loc_age_max] [smallint] NOT NULL,
 CONSTRAINT [watch_location_PK] PRIMARY KEY CLUSTERED 
(
	[loc_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[member]    Script Date: 4/18/2018 9:06:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[member](
	[member_id] [varchar](11) NOT NULL,
	[member_fName] [varchar](20) NOT NULL,
	[member_lName] [varchar](30) NOT NULL,
	[barcode] [varchar](6) NOT NULL,
	[pin] [varchar](4) NOT NULL,
	[phone] [varchar](13) NOT NULL,
	[active] [bit] NULL,
 CONSTRAINT [member_PK] PRIMARY KEY CLUSTERED 
(
	[member_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[barcode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[member_child]    Script Date: 4/18/2018 9:06:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[member_child](
	[child_id] [int] NOT NULL,
	[member_id] [varchar](11) NOT NULL,
 CONSTRAINT [member_child_PK] PRIMARY KEY CLUSTERED 
(
	[child_id] ASC,
	[member_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[signin_detail]    Script Date: 4/18/2018 9:06:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[signin_detail](
	[signin_id] [int] NOT NULL,
	[child_id] [int] NOT NULL,
	[watch_location] [int] NOT NULL,
 CONSTRAINT [signin_detail_PK] PRIMARY KEY CLUSTERED 
(
	[signin_id] ASC,
	[child_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[signin_log]    Script Date: 4/18/2018 9:06:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[signin_log](
	[signin_id] [int] IDENTITY(1,1) NOT NULL,
	[member_id] [varchar](11) NOT NULL,
	[time_in] [datetime] NOT NULL,
	[time_out] [datetime] NULL,
	[band_number] [smallint] NOT NULL,
	[employee] [int] NULL,
 CONSTRAINT [signin_log_PK] PRIMARY KEY CLUSTERED 
(
	[signin_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[employee] ADD  CONSTRAINT [DF_employee_admin]  DEFAULT ((0)) FOR [emp_admin_flag]
GO
ALTER TABLE [dbo].[employee] ADD  CONSTRAINT [DF_employee_emp_resetflag]  DEFAULT ((0)) FOR [emp_reset_flag]
GO
ALTER TABLE [dbo].[member_child]  WITH CHECK ADD  CONSTRAINT [member_child_child_FK] FOREIGN KEY([child_id])
REFERENCES [dbo].[child] ([child_id])
GO
ALTER TABLE [dbo].[member_child] CHECK CONSTRAINT [member_child_child_FK]
GO
ALTER TABLE [dbo].[member_child]  WITH CHECK ADD  CONSTRAINT [member_child_member_FK] FOREIGN KEY([member_id])
REFERENCES [dbo].[member] ([member_id])
GO
ALTER TABLE [dbo].[member_child] CHECK CONSTRAINT [member_child_member_FK]
GO
ALTER TABLE [dbo].[signin_detail]  WITH CHECK ADD  CONSTRAINT [signin_detail_child_FK] FOREIGN KEY([child_id])
REFERENCES [dbo].[child] ([child_id])
GO
ALTER TABLE [dbo].[signin_detail] CHECK CONSTRAINT [signin_detail_child_FK]
GO
ALTER TABLE [dbo].[signin_detail]  WITH CHECK ADD  CONSTRAINT [signin_detail_location_FK] FOREIGN KEY([watch_location])
REFERENCES [dbo].[location] ([loc_id])
GO
ALTER TABLE [dbo].[signin_detail] CHECK CONSTRAINT [signin_detail_location_FK]
GO
ALTER TABLE [dbo].[signin_detail]  WITH CHECK ADD  CONSTRAINT [signin_detail_log_FK] FOREIGN KEY([signin_id])
REFERENCES [dbo].[signin_log] ([signin_id])
GO
ALTER TABLE [dbo].[signin_detail] CHECK CONSTRAINT [signin_detail_log_FK]
GO
ALTER TABLE [dbo].[signin_log]  WITH CHECK ADD  CONSTRAINT [signin_log_employee_FK] FOREIGN KEY([employee])
REFERENCES [dbo].[employee] ([emp_id])
GO
ALTER TABLE [dbo].[signin_log] CHECK CONSTRAINT [signin_log_employee_FK]
GO
ALTER TABLE [dbo].[signin_log]  WITH CHECK ADD  CONSTRAINT [signin_log_member_FK] FOREIGN KEY([member_id])
REFERENCES [dbo].[member] ([member_id])
GO
ALTER TABLE [dbo].[signin_log] CHECK CONSTRAINT [signin_log_member_FK]
GO
/****** Object:  StoredProcedure [dbo].[p_child_get]    Script Date: 4/18/2018 9:06:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Christian Ore
-- Create date: 3/28/2018
-- Description:	Get a child's information.
-- =============================================
CREATE PROCEDURE [dbo].[p_child_get]
	@child_id INT
AS
BEGIN
	SELECT *
	FROM child WITH(NOLOCK)
	WHERE child_id = @child_id 
END
GO
/****** Object:  StoredProcedure [dbo].[p_child_insert]    Script Date: 4/18/2018 9:06:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[p_child_insert]
	@fname VARCHAR(20),
	@lname VARCHAR(30),
	@birth_dt DATE,
	@member_id VARCHAR(11)

AS
BEGIN

	BEGIN TRANSACTION

	IF (SELECT COUNT(member_id) FROM member WITH(NOLOCK) WHERE member_id = @member_id) < 1
		SELECT -1
	ELSE
	BEGIN
		-- Lock the table so that the id can be retrieved.
		INSERT INTO child WITH(TABLOCK) VALUES( @fname, @lname, @birth_dt) 

		DECLARE @child_id INT = (SELECT MAX(child_id) FROM child WITH(NOLOCK));
	
		IF @child_id IS NULL
			ROLLBACK TRANSACTION
		ELSE
		BEGIN
			INSERT INTO  member_child WITH(ROWLOCK) VALUES(@child_id, @member_id)
			SELECT @child_id 
		END

	END
	COMMIT TRANSACTION

END
GO
/****** Object:  StoredProcedure [dbo].[p_child_update]    Script Date: 4/18/2018 9:06:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Christian Ore
-- Create date: 3/28/2018
-- Description:	Update a child record
-- =============================================
CREATE PROCEDURE [dbo].[p_child_update] 
	@fname		VARCHAR(50),
	@lname		VARCHAR(200),
	@bday		DATE,
	@child_id	INT
AS
BEGIN
	UPDATE child WITH(ROWLOCK) SET
		child_fName = @fname,
		child_lName = @lName,
		birthdate = @bday
	WHERE
		child_id = @child_id 

END
GO
/****** Object:  StoredProcedure [dbo].[p_employee_get]    Script Date: 4/18/2018 9:06:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Christian Ore
-- Create date: 4/17/2018
-- Description:	Select employee data for administration portal 
-- =============================================
CREATE PROCEDURE [dbo].[p_employee_get] 
	@id INT = NULL 
AS
BEGIN
	
	IF OBJECT_ID(N'tempdb..#tmp_emp') IS NOT NULL
		DROP TABLE #tmp_emp

	SELECT
		emp_id,
		emp_firstname,
		emp_lastname,
		CAST(DECRYPTBYASYMKEY(ASYMKEY_ID('YMCAKey'), emp_username, N'llb2cxy') AS VARCHAR) AS 'emp_username'
	INTO #tmp_emp
	FROM employee WITH(NOLOCK) 

	IF @id IS NOT NULL
		DELETE FROM #tmp_emp WHERE emp_id <> @id 
	
	SELECT *
	FROM #tmp_emp	

END
GO
/****** Object:  StoredProcedure [dbo].[p_employee_insert]    Script Date: 4/18/2018 9:06:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Christian Ore
-- Create date: 4/15/2018
-- Description:	Create a new employee in the database
-- =============================================
CREATE PROCEDURE [dbo].[p_employee_insert]
	@firstname VARCHAR(100),
	@lastname VARCHAR(100),
	@username VARCHAR(100),
	@password VARCHAR(100),
	@admin	BIT = 0
	
	
AS
BEGIN
	DECLARE @EncryptionKey INT = AsymKey_ID('YMCAKey'),
		@exists INT = 0
	SELECT 
		@exists = COUNT(8)
	FROM employee WITH(NOLOCK)
	WHERE 
		@username = CAST(DECRYPTBYASYMKEY(@EncryptionKey, emp_username, N'llb2cxy') AS VARCHAR) 
	
	IF @exists = 0 BEGIN
	INSERT INTO employee WITH(ROWLOCK)
	(emp_firstname, 
	emp_lastname, 
	emp_username, 
	emp_password, 
	emp_reset_flag, 
	emp_admin_flag,
	emp_blocked)
	VALUES(
		@firstname,
		@lastname,
		ENCRYPTBYASYMKEY(@EncryptionKey, @username),
		ENCRYPTBYASYMKEY(@EncryptionKey, @password),
		0,
		@admin,
		0
	)
		SELECT 0
	END
	ELSE
		SELECT -1 
END
GO
/****** Object:  StoredProcedure [dbo].[p_employee_modify]    Script Date: 4/18/2018 9:06:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[p_employee_modify] 
	@id int,
	@firstname varchar = NULL,
	@lastname varchar = NULL,
	@blocked bit = NULL,
	@admin bit = NULL,
	@reset bit = NULL
AS
BEGIN
	UPDATE employee WITH(ROWLOCK) SET
		emp_firstname = ISNULL(@firstname, emp_firstname),
		emp_lastname = ISNULL(@lastname, emp_lastname),
		emp_blocked = ISNULL(@blocked, emp_blocked),
		emp_admin_flag = ISNULL(@admin, emp_admin_flag),
		emp_reset_flag = ISNULL(@reset, emp_reset_flag)
	WHERE emp_id = @id
END
GO
/****** Object:  StoredProcedure [dbo].[p_employee_update_login]    Script Date: 4/18/2018 9:06:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
Author		Christian Ore
Date		4/15/2018
Description Modifies the login credentials for the given employee id
*/
CREATE PROCEDURE [dbo].[p_employee_update_login]
	
	@password VARCHAR(50),
	@id		 INT
AS

BEGIN

	UPDATE employee WITH(ROWLOCK) SET
		
		emp_password =ENCRYPTBYASYMKEY(ASYMKEY_ID('YMCAKey'), @password)
	WHERE
		emp_id = @id 

END 
GO
/****** Object:  StoredProcedure [dbo].[p_employee_validate]    Script Date: 4/18/2018 9:06:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Christian Ore
-- Create date: 4/15/2018
-- Description:	Validates a login attempt
-- =============================================
CREATE PROCEDURE [dbo].[p_employee_validate]
	@user	VARCHAR(50),
	@pass	VARCHAR(50)

AS
BEGIN
	DECLARE 
		@secret nvarchar(50) = N'llb2cxy',
		@key INT = ASYMKEY_ID('YMCAKey'),
		@reset BIT = 0,
		@id INT = 0,
		@block BIT = 0,
		@admin BIT = 0

	SELECT 
		@id = emp_id,
		@reset = emp_reset_flag,
		@block = emp_blocked,
		@admin = emp_admin_flag
	FROM employee WITH(NOLOCK)
	WHERE
		@user = CAST(DECRYPTBYASYMKEY(@key, emp_username, @secret ) AS VARCHAR)
		AND @pass = CAST(DECRYPTBYASYMKEY(@key, emp_password, @secret ) AS VARCHAR)

	IF @id > 0 BEGIN
		IF @block = 1 -- User is blocked
			SELECT -3
		ELSE IF @reset = 1 -- User needs to be reset 
			SELECT -2
		ELSE IF @admin = 1	-- User is an admin
			SELECT 1
		ELSE
			SELECT 0 -- User is a normal user
	END
	ELSE
		SELECT -1 -- User is not a valid user
END
GO
/****** Object:  StoredProcedure [dbo].[p_location_get]    Script Date: 4/18/2018 9:06:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Christian
-- Create date: 3/31/2018
-- Description:	This procedure will get the locations for a specific branch
-- =============================================
CREATE PROCEDURE [dbo].[p_location_get] 
	@id INT = NULL
	
AS
BEGIN
	IF @id IS NULL BEGIN
		SELECT *
		FROM location WITH(NOLOCK)
	END
	ELSE BEGIN
		SELECT *
		FROM location WITH(NOLOCK)
		WHERE loc_id = @id
	END
END
GO
/****** Object:  StoredProcedure [dbo].[p_location_save]    Script Date: 4/18/2018 9:06:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Christian Ore
-- Create date: 3/29/2018
-- Description:	Save a watch location
-- =============================================
CREATE PROCEDURE [dbo].[p_location_save] 
	@id		INT = -1,	
	@name	VARCHAR(30),
	@min	INT,
	@max	INT
AS
BEGIN
	IF @id < 0 BEGIN
		INSERT INTO location WITH(ROWLOCK)
		VALUES(@name, @min, @max)
	END
	ELSE BEGIN
		UPDATE location WITH(ROWLOCK) SET
			loc_name = @name,
			loc_age_min = @min,
			loc_age_max = @max
		WHERE
			loc_id = @id 
	END
END
GO
/****** Object:  StoredProcedure [dbo].[p_member_child_attach]    Script Date: 4/18/2018 9:06:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Christian Ore
-- Create date: 03/18/2018
-- Description:	Attachs a child to a member to be able to be signed in
-- =============================================
CREATE PROCEDURE [dbo].[p_member_child_attach]
	@member_id VARCHAR(11),
	@child_id INT
AS
BEGIN
	
	INSERT INTO member_child WITH(ROWLOCK) VALUES(@child_id, @member_id)

END
GO
/****** Object:  StoredProcedure [dbo].[p_member_child_get]    Script Date: 4/18/2018 9:06:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
Author:			Christian Ore
Date:			3/11/2018
Description:	Find all of the children that are attached
to this member to be dropped off at the YMCA.
*/
CREATE PROCEDURE [dbo].[p_member_child_get]
	@member_id VARCHAR(11)
AS

BEGIN

SELECT 
	child.*
FROM child WITH(NOLOCK)
INNER JOIN member_child WITH(NOLOCK) ON
	child.child_id = member_child.child_id
WHERE
	member_child.member_id = @member_id

END
GO
/****** Object:  StoredProcedure [dbo].[p_member_get]    Script Date: 4/18/2018 9:06:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
	AUTHOR:		CHRISTIAN ORE
	DATE:		3/25/2018
	DESCRIPTION	
	RETURNS MEMBER INFORMATION FOR SPECIFIED CRITERIA
*/

CREATE PROCEDURE [dbo].[p_member_get]
	@member_id	VARCHAR(11) = NULL,
	@barcode	VARCHAR(6) = NULL,
	@pin		VARCHAR(4) = NULL
AS

BEGIN
	IF @member_id IS NOT NULL
		SELECT *
		FROM member WITH(NOLOCK)
		WHERE member_id = @member_id
	ELSE IF @barcode IS NOT NULL AND @pin IS NOT NULL
		SELECT *
		FROM member WITH(NOLOCK)
		WHERE barcode = @barcode AND pin = @pin

END
GO
/****** Object:  StoredProcedure [dbo].[p_member_save]    Script Date: 4/18/2018 9:06:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
	AUTHOR:		CHRISTIAN ORE
	DATE:		3/25/2018
	DESCRIPTION
	THIS STORED PROCEDURE WILL SAVE A MEMBER TO THE DATABASE OR ADD THEM IF THEY ARE NOT THERE.

*/

CREATE PROCEDURE [dbo].[p_member_save]
	@member_id		VARCHAR(11),
	@first_name		VARCHAR(20),
	@last_name		VARCHAR(30),
	@barcode		VARCHAR(6),
	@pin			VARCHAR(4),
	@phone			VARCHAR(13),
	@active			BIT
AS

BEGIN
	
	DECLARE @memberCount INT	 
	SELECT
		@memberCount = COUNT(member_id)
	FROM
		member WITH(NOLOCK)
	WHERE
		member_id = @member_id
	
	IF @memberCount = 0
		INSERT INTO member WITH(ROWLOCK) VALUES(@member_id, @first_name, @last_name, @barcode, @pin, @phone, @active)
	ELSE IF @memberCount = 1
		UPDATE member WITH(ROWLOCK)
		SET 
			member_fName = @first_name,
			member_lName = @last_name,
			barcode = @barcode,
			pin = @pin,
			phone = @phone,
			active = @active 
		WHERE
			member_id = @member_id

END
GO
/****** Object:  StoredProcedure [dbo].[p_signin_detail_add]    Script Date: 4/18/2018 9:06:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Christian Ore
-- Create date: 3/28/2018
-- Description:	Add a detail record to an open signin
-- =============================================
CREATE PROCEDURE [dbo].[p_signin_detail_add] 
	@signin_id		INT,
	@child_id		INT,
	@location_id	INT

AS
BEGIN
	DECLARE 
		@open		INT = 0,
		@childCount	INT = 0
		-- Is the current signing log open?
	SELECT 
		@open = COUNT(signin_id) 
	FROM signin_log WITH(NOLOCK)
	WHERE 
		signin_id = @signin_id
		AND CAST(time_in AS DATE) = CAST(GETDATE() AS DATE)
		AND time_out IS NULL
		-- If there is one open
	IF @open = 1
	BEGIN
	-- Is this child already signed in?
		SELECT
			@childCount = COUNT(child_id)
		FROM
			signin_detail WITH(NOLOCK)
		WHERE
			signin_id = @signin_id 
			AND child_id = @child_id
	-- If not, sign him/her in.
		IF @childCount = 0
		INSERT INTO signin_detail WITH(ROWLOCK) VALUES(@signin_id, @child_id, @location_id)
	END
END
GO
/****** Object:  StoredProcedure [dbo].[p_signin_detail_update]    Script Date: 4/18/2018 9:06:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Christian Ore
-- Create date: 3/28/2018
-- Description:	Update the location of a child in the details table
-- =============================================
CREATE PROCEDURE [dbo].[p_signin_detail_update] 
	@signin_id		INT = 0,
	@child_id		INT = 0,
	@location_id	INT = 0
AS
BEGIN

	IF dbo.f_signin_open(@signin_id) = 0
		SELECT -1
	ELSE
		UPDATE signin_detail WITH(ROWLOCK) SET
			watch_location = @location_id
		WHERE
			signin_id = @signin_id
			AND child_id = @child_id 
END
GO
/****** Object:  StoredProcedure [dbo].[p_signin_in]    Script Date: 4/18/2018 9:06:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Christian Ore
-- Create date: 3/28/2018
-- Description:	Create a new sign in log
-- =============================================
CREATE PROCEDURE [dbo].[p_signin_in]
	@member_id		VARCHAR(11),
	@band_number	INT = -1 OUT, 
	@signin_id		INT = -1 OUT
AS
BEGIN

	DECLARE 
		@currBand		INT = 0
	-- Find any current open sign ins
	SELECT 
		@currBand = ISNULL(band_number,0)
	FROM signin_log WITH(ROWLOCK)
	WHERE
		member_id = @member_id
		AND time_in IS NOT NULL
		AND time_out IS NULL
		AND CAST(time_in AS DATE) <= CAST(GETDATE() AS DATE)
	-- If not already signed in
	IF @currBand = 0 BEGIN
	BEGIN TRANSACTION TABLOCK
	-- Get their band number	
		SELECT
			@band_number = ISNULL(MAX(band_number), 999) + 1
		FROM signin_log 

		IF @band_number = 10000 
			SET @band_number = 1000
	
		INSERT INTO signin_log 
		VALUES(@member_id, GETDATE(), NULL, @band_number, NULL)
	-- Get the sign in id just issued
		SELECT
			@signin_id = signin_id 
		FROM signin_log
		WHERE
			member_id = @member_id
			AND CAST(GETDATE() AS DATE) = CAST(time_in AS DATE)
	-- If there is not one, rollback
		IF @signin_id < 0
			ROLLBACK TRANSACTION
		ELSE
		BEGIN
			COMMIT TRANSACTION
		END
	END
	ELSE BEGIN
		SELECT @currBand
	END
END
GO
/****** Object:  StoredProcedure [dbo].[p_signin_modify]    Script Date: 4/18/2018 9:06:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[p_signin_modify]

	@id INT,
	@member VARCHAR(11) = NULL,
	@in DATETIME = NULL,
	@out DATETIME = NULL,
	@band INT = NULL,
	@emp VARCHAR(200) = NULL
AS BEGIN
UPDATE signin_log WITH(ROWLOCK) SET
	member_id = ISNULL(@member, member_id),
	time_in = ISNULL(@in, time_in),
	time_out = ISNULL(@out, time_out),
	band_number = ISNULL(@band, band_number),
	employee = ISNULL(@emp, employee)
WHERE signin_id = @id
END
GO
/****** Object:  StoredProcedure [dbo].[p_signin_out]    Script Date: 4/18/2018 9:06:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Christian Ore
-- Create date: 3/28/2018
-- Description:	Update signing log with a new signout 
-- =============================================
CREATE PROCEDURE [dbo].[p_signin_out]
	@band INT = -1

AS
BEGIN
	DECLARE @id INT = -1
	SELECT 
		@id = signin_id
	FROM	signin_log WITH(NOLOCK)
	WHERE	
		band_number = @band
		AND CAST(time_in AS DATE) = CAST(GETDATE() AS DATE)
		AND time_out IS NULL 

	IF @id > 0
	BEGIN
		UPDATE signin_log WITH(ROWLOCK) SET
			time_out =  GETDATE()
		WHERE
			signin_id = @id 
		SELECT 1;
	END 
	ELSE
		SELECT -1;
END
GO
/****** Object:  StoredProcedure [dbo].[p_signin_remove]    Script Date: 4/18/2018 9:06:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Christian Ore
-- Create date: 3/31/2018
-- Description:	Remove a signin
-- =============================================
CREATE PROCEDURE [dbo].[p_signin_remove] 
	@id INT
AS
BEGIN
	DELETE FROM signin_detail WHERE signin_id = @id
	DELETE FROM signin_log WHERE signin_id = @id 
END
GO
/****** Object:  StoredProcedure [dbo].[r_children_signedin]    Script Date: 4/18/2018 9:06:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- AUTHOR:		Christian Ore
-- DATE:		4/12/2018
-- DESCRIPTION:	Shows the currently signed in children using the YMCA.
CREATE PROCEDURE [dbo].[r_children_signedin]
AS
BEGIN
SELECT 
	child.child_id,
	child_fName,
	child_lName,
	birthdate,
	loc_name
FROM signin_detail
INNER JOIN child ON
	child.child_id = signin_detail.child_id 
INNER JOIN location ON
	location.loc_id = watch_location
INNER JOIN signin_log ON
	signin_detail.signin_id = signin_log.signin_id
WHERE
	CAST(time_in AS DATE) = CAST(GETDATE() AS DATE)
	AND time_out IS NULL  
END
GO
/****** Object:  StoredProcedure [dbo].[r_interval]    Script Date: 4/18/2018 9:06:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- AUTHOR:		Christian Ore
-- DATE:		4/11/2018
-- DESCRIPTION:	Show the children signed in during a given time frame by a certain interval.
CREATE PROCEDURE [dbo].[r_interval]
	@start		DATETIME,
	@end		DATETIME,
	@interval	INT = 60,
	@location	INT
AS
BEGIN

DECLARE
	@partitions  INT = 1,
	@count		INT = 0,
	@tmptime	DATETIME,
	@children	INT,
	@minutes INT = 0;

-- Determine the amount of minutes for the difference.
SET @minutes = (DATEPART(HOUR, @end) - DATEPART(HOUR, @start)) * 60
SET @minutes = @minutes + (DATEPART(MINUTE, @end) - DATEPART(MINUTE, @start))
-- Find the partitions
SET @partitions = @minutes / @interval

IF OBJECT_ID(N'tempdb..#interval') IS NOT NULL
	DROP TABLE #interval

CREATE TABLE #interval
(
	id INT ,
	time DATETIME,
	amount INT 
)
-- Create the partitions
WHILE(@count < @partitions)
BEGIN
--Each partition is a factor of the previous amount
SET @tmpTime = DATEADD(MINUTE, @interval * @count, @start);
-- Find the information for people who signed in or out during the partition bounds.
SET @children = (
	SELECT COUNT(child_id) 
	FROM signin_log
	INNER JOIN signin_detail ON
	signin_detail.signin_id = signin_log.signin_id
	WHERE 
		--(time_in BETWEEN @tmpTime AND DATEADD(MINUTE, @interval , @tmpTime)
		--OR time_out BETWEEN @tmpTime AND DATEADD(MINUTE, @interval , @tmpTime))
		time_in < DATEADD(MINUTE, @interval , @tmpTime)
		AND time_out >= @tmptime
		AND watch_location = @location 
)
-- Add this into our list.
INSERT INTO #interval 
VALUES(@count, @tmpTime, @children)
SET @count += 1
END 

SELECT *
FROM #interval


END
GO
/****** Object:  StoredProcedure [dbo].[r_members]    Script Date: 4/18/2018 9:06:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
	AUTHOR: Kayla Sparklin
	DATE: 04/07/2018
	DESCRIPTION: Pull information for registered members based on current YMCA membership
				 status.

*/
CREATE PROCEDURE [dbo].[r_members] 
	@active BIT = NULL
AS
	BEGIN
	
	IF @active IS NULL
		SElECT *
		FROM member WITH(NOLOCK)
	ELSE
		SELECT *
		FROM member WITH(NOLOCK)
		WHERE active = @active 
	
END 
GO
USE [master]
GO
ALTER DATABASE [ymca_child_watch] SET  READ_WRITE 
GO
