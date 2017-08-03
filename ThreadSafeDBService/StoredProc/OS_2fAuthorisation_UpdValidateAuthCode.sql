USE [MMSGCommon]
GO

/****** Object: SqlProcedure [dbo].[OS_2fAuthorisation_UpdValidateAuthCode] Script Date: 8/3/2017 1:39:05 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[OS_2fAuthorisation_UpdValidateAuthCode]
	 @iSystemID INT
	,@iCallingContext INT
	,@sAccountID VARCHAR(255)
	,@sMobileNo VARCHAR(20)
	,@sAuthorisationCode VARCHAR(128)

AS
BEGIN
Declare @Object as Int;
Declare @ResponseText as Varchar(8000);
SET @BaseUrl = "http://localhost:1234/api/OS_2fAuthorisation_UpdValidateAuthCode?"
SET @QueryString = 'iSystemID=' + @iSystemID + '&' +  'iCallingContext=' + @iCallingContext + '&' +'sAccountID=' + @sAccountID + '&' +'sMobileNo=' + @sMobileNo + '&' +'sAuthorisationCode=' + @sAuthorisationCode 
SET @DBThreadServiceUrl = @BaseUrl + @QueryString;

Exec sp_OACreate 'MSXML2.XMLHTTP', @Object OUT;
Exec sp_OAMethod @Object, 'open', NULL, 'get',
	'@DBThreadServiceUrl',
 'false'
Exec sp_OAMethod @Object, 'send'
Exec sp_OAMethod @Object, 'responseText', @ResponseText OUTPUT
 
Select @ResponseText
 
Exec sp_OADestroy @Object
	END