CREATE TABLE [dbo].[Event]
(
	[Id] int primary key identity,
	[Name] nvarchar(120) NOT NULL,
	[Description] nvarchar(max) NOT NULL,
	[LayoutId] int NOT NULL,
	[DateTimeStart] datetime2 NOT NULL,
	[DateTimeEnd] datetime2 NOT NULL,
	[URL] nvarchar(2000)
)
