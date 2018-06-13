--Create your database named Tracker - or use whichever you want, just change the [ ] to your database name.
--After you have created the database, run this set of SQL commands to get yourself setup with all you need to run with my MVC_Angular4_Demo
USE [Tracker]
GO

--Add new table to store all tickets
CREATE TABLE [dbo].[Issues](
	[IssueID] [int] IDENTITY(1,1) NOT NULL,
	[DateAdded] [date] NOT NULL,
	[Active] [bit] NOT NULL,
	[IssueType] [int] NOT NULL,
	[IssueText] [varchar](500) NULL,
	[TextRecieved] [varchar] (255) NULL,
	[TextWanted] [varchar] (255) NULL,
PRIMARY KEY CLUSTERED 
(
	[IssueID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

--Add new table for all types of issue
CREATE TABLE [dbo].[IssueType](
	[IssueTypeID] [int] NOT NULL,
	[IssueName] [varchar](15) NULL, 
	[IssueDescription] [varchar](50) NULL
PRIMARY KEY CLUSTERED 
(
	[IssueTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

--Add the Foreign key 
ALTER TABLE [dbo].[Issues]  WITH CHECK ADD  CONSTRAINT [FK_IssueTypes] FOREIGN KEY([IssueType])
REFERENCES [dbo].[IssueType] ([IssueTypeID])
GO

ALTER TABLE [dbo].[Issues] CHECK CONSTRAINT [FK_IssueTypes]
GO


--Stored Procedure which pulls in all tickets
CREATE PROCEDURE 
[dbo].[spGetAllTickets]
AS
SELECT 
	IssueID, 
	CONVERT(Varchar, DateAdded, 10) AS DateAdded,
	Active, 
	IT.IssueDescription, 
	IssueText,
	TextRecieved,
	TextWanted	
FROM Issues I INNER JOIN IssueType IT ON IT.IssueTypeID = I.IssueType
ORDER BY DateAdded ASC


--Stored Procedure which adds a new ticket
CREATE PROCEDURE [dbo].[spNewIssue]
	@DateAdded	DATE, 
    @IssueType	INT, 
	@IssueText	varchar(500),
	@TextGot	varchar(255) NULL,
	@TextWanted	varchar (255) NULL
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO ISSUES (DateAdded, Active, IssueType, IssueText, TextRecieved, TextWanted, ClosedDesc)
	VALUES (@DateAdded, 1, @IssueType, @IssueText, @TextGot, @TextWanted, NULL)	

	SELECT @@IDENTITY;
END


--Stored Procedure which closes a ticket
CREATE PROCEDURE [dbo].[spCloseTicket]
	@IssueID	INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE 
	ISSUES
	SET Active=0
	WHERE IssueID = @IssueID
END


--default IssueTypes
INSERT INTO IssueType VALUES (1, 'BUG', 'Error')
INSERT INTO IssueType VALUES (2, 'ACCESS', 'Access/Permissions')
INSERT INTO IssueType VALUES (3, 'REPORT', 'Reporting')
INSERT INTO IssueType VALUES (4, 'HELP', 'General Help')

--default data so a user can immediately view or close tickets
INSERT INTO ISSUES (DateAdded, Active, IssueType, IssueText, TextRecieved, TextWanted) VALUES ('2018-06-01',1,2,'Need to give permissions to Test.User to access application',null, null)
INSERT INTO ISSUES (DateAdded, Active, IssueType, IssueText, TextRecieved, TextWanted) VALUES ('2018-05-04',0,4,'Install and configure everything on my machine', null, null)
INSERT INTO ISSUES (DateAdded, Active, IssueType, IssueText, TextRecieved, TextWanted) VALUES ('2018-05-20',1,1,'Is all broken. Fix it asap', 'I logged in and it was like blank','I should not be blank, it should have my old data')
INSERT INTO ISSUES (DateAdded, Active, IssueType, IssueText, TextRecieved, TextWanted) VALUES ('2018-06-02',1,4,'How does this angular 4 stuff work anyway? Please help.', null, null)
INSERT INTO ISSUES (DateAdded, Active, IssueType, IssueText, TextRecieved, TextWanted) VALUES ('2018-06-04',1,3,'I want to see reporting on the issues!', null, null)