USE [MMSGCommon]
GO

/****** Object: SqlProcedure [dbo].[OS_2fAuthorisation_InsAuthCode] Script Date: 8/4/2017 4:44:05 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



ALTER PROCEDURE [dbo].[OS_2fAuthorisation_InsAuthCode]
	 @iSystemID INT
	,@iCallingContext INT
	,@sAccountID VARCHAR(255)
	,@sMobileNo VARCHAR(20)
	,@sAuthorisationCode VARCHAR(128)
	,@iExpiryMinutes INT
AS
BEGIN
Declare @Object as Int;
Declare @ResponseText as Varchar(8000);
DECLARE @BaseUrl nvarchar(250);
DECLARE @QueryString nvarchar(250);
DECLARE @DBThreadServiceUrl nvarchar(500);

SET @BaseUrl = 'http://10.65.120.120:1234/api/OS_2fAuthorisation_InsAuthCode?'
SET @QueryString = 'iSystemID=' + CONVERT(VARCHAR(10), @iSystemID) + '&' +  'iCallingContext=' + CONVERT(VARCHAR(10), @iCallingContext) + '&' +'sAccountID=' + @sAccountID + '&' +'sMobileNo=' + @sMobileNo + '&' +'sAuthorisationCode=' + @sAuthorisationCode + '&' + 'iExpiryMinutes=' + CONVERT(VARCHAR(10), @iExpiryMinutes)
SET @DBThreadServiceUrl = @BaseUrl + @QueryString;
Select @DBThreadServiceUrl
Exec sp_OACreate 'MSXML2.ServerXMLHTTP', @Object OUT;
Exec sp_OAMethod @Object, 'open', NULL, 'get', @DBThreadServiceUrl,'false'
Exec sp_OAMethod @Object, 'send'
Exec sp_OAMethod @Object, 'responseText', @ResponseText OUTPUT
Select @ResponseText
Exec sp_OADestroy @Object
END
