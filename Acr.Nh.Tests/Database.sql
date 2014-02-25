USE [acrframework]

IF OBJECT_ID('dbo.Attachments','U') IS NOT NULL BEGIN
    DROP TABLE [dbo].[Attachments]
END
IF OBJECT_ID('My.Attachments','U') IS NOT NULL BEGIN
    DROP TABLE [My].[Attachments]
END
IF OBJECT_ID('dbo.Todos','U') IS NOT NULL BEGIN
    DROP TABLE [dbo].[Todos]
END
IF OBJECT_ID('dbo.Users','U') IS NOT NULL BEGIN
    DROP TABLE [dbo].[Users]
END
CREATE TABLE [dbo].[Todos](
    [TodoID] [int] NOT NULL,
    [Details] [nvarchar](max) NOT NULL,
    [DateCreated] [datetimeoffset](7) NOT NULL,
    [DateCompleted] [datetimeoffset](7) NULL,
    [UserID] [int] NOT NULL,
 CONSTRAINT [PK_Todos] PRIMARY KEY CLUSTERED 
(
    [TodoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


CREATE TABLE [dbo].[Attachments](
[AttachmentID] [int] NOT NULL,
[Data] [varbinary](max) NOT NULL,
[FileSize] [bigint] NOT NULL,
[FileName] [varchar](255) NOT NULL,
CONSTRAINT [PK_Attachments] PRIMARY KEY CLUSTERED
(
[AttachmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

CREATE TABLE [My].[Attachments](
[AttachmentID] [int] NOT NULL,
[Data] [varbinary](max) NOT NULL,
[FileSize] [bigint] NOT NULL,
[FileName] [varchar](255) NOT NULL,
CONSTRAINT [PK_Attachments] PRIMARY KEY CLUSTERED
(
[AttachmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


CREATE TABLE [dbo].[Users](
[UserID] [int] NOT NULL,
[UserName] [nvarchar](20) NOT NULL,
[Password] [varbinary](50) NULL,
[FirstName] [nvarchar](50) NOT NULL,
[LastName] [nvarchar](50) NOT NULL,
CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED
(
[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

INSERT INTO [acrframework].[dbo].[Attachments](
[AttachmentID],
[Data],
[FileSize],
[FileName]
)
SELECT TOP 10
[DocumentID]
, [BlobData]
,[FileSize]
,[FileName]
FROM [Csc].[dbo].[Documents]
ORDER BY 1 ASC

INSERT INTO [acrframework].[my].[Attachments](
[AttachmentID],
[Data],
[FileSize],
[FileName]
)
SELECT TOP 10
[DocumentID]
, [BlobData]
,[FileSize]
,[FileName]
FROM [Csc].[dbo].[Documents]
ORDER BY 1 DESC


INSERT INTO [acrframework].[dbo].[Todos](
[TodoID],
[Details],
[DateCreated],
[DateCompleted],
[UserID]
)
SELECT TOP 100
[TaskID],
[Details],
[DateCreated],
[DateFiled],
1
FROM [Csc].[dbo].[Tasks]
order by 1 desc
