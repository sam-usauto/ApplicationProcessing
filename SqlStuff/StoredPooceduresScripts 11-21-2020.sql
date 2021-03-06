USE [ScoringDBProd_Dev_Sam]
GO
/****** Object:  Table [dbo].[ApplicationFlow]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationFlow](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FlowName] [varchar](100) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_ApplicationFlow] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApplicationFlowStep]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationFlowStep](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationFlowId] [int] NOT NULL,
	[StepName] [varchar](100) NULL,
	[StepOrder] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ExecuteUrl] [varchar](200) NULL,
 CONSTRAINT [PK_ApplicationFlowStep] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [logs].[ApplicationFlowStepResult]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [logs].[ApplicationFlowStepResult](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClientApplicationId] [int] NOT NULL,
	[ApplicationFlowStepId] [int] NOT NULL,
	[IsCompleted] [bit] NOT NULL,
	[StepOrder] [int] NOT NULL,
	[Request1] [varchar](max) NULL,
	[Response1] [varchar](max) NULL,
	[Request2] [varchar](max) NULL,
	[Response2] [varchar](max) NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_ApplicationFlowStepResult] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [logs].[ClientApplication]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [logs].[ClientApplication](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationFlowID] [int] NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[PhoneNumber] [varchar](max) NULL,
	[Email] [varchar](max) NULL,
	[Last4Ssn] [char](4) NULL,
	[CreditScoreAppId] [int] NULL,
	[ApplicationId] [int] NULL,
	[PayingCapacityId] [int] NULL,
	[AddrId] [int] NULL,
	[CustId] [int] NULL,
	[EmploymentId] [int] NULL,
	[OriginalApp] [varchar](max) NULL,
	[InitAppSaveError] [varchar](max) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_ClientApplication] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[ApplicationFlowStep] ADD  CONSTRAINT [DF_ApplicationFlowStep_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [logs].[ApplicationFlowStepResult] ADD  CONSTRAINT [DF_ApplicationFlowStepResult_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [logs].[ClientApplication] ADD  CONSTRAINT [DF_ClientApplication_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [logs].[ClientApplication] ADD  CONSTRAINT [DF_ClientApplication_ModifiedOn]  DEFAULT (getdate()) FOR [ModifiedOn]
GO
ALTER TABLE [dbo].[ApplicationFlowStep]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationFlowStep_ApplicationFlow] FOREIGN KEY([ApplicationFlowId])
REFERENCES [dbo].[ApplicationFlow] ([Id])
GO
ALTER TABLE [dbo].[ApplicationFlowStep] CHECK CONSTRAINT [FK_ApplicationFlowStep_ApplicationFlow]
GO
ALTER TABLE [logs].[ApplicationFlowStepResult]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationFlowStepResult_ApplicationFlowStep] FOREIGN KEY([ApplicationFlowStepId])
REFERENCES [dbo].[ApplicationFlowStep] ([Id])
GO
ALTER TABLE [logs].[ApplicationFlowStepResult] CHECK CONSTRAINT [FK_ApplicationFlowStepResult_ApplicationFlowStep]
GO
ALTER TABLE [logs].[ApplicationFlowStepResult]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationFlowStepResult_ClientApplication] FOREIGN KEY([ClientApplicationId])
REFERENCES [logs].[ClientApplication] ([Id])
GO
ALTER TABLE [logs].[ApplicationFlowStepResult] CHECK CONSTRAINT [FK_ApplicationFlowStepResult_ClientApplication]
GO
/****** Object:  StoredProcedure [app].[GetApplicationFlowSteps]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
------------------------------------------------------------------------------------------------------------------------
-- Date Created: Saturday, November 16, 2020
-- Created By:   Sam Aloni
-- Get the list of steps by LogID
-- EXEC [app].[GetApplicationFlowSteps] 10; 
------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [app].[GetApplicationFlowSteps]
	@LogId int

AS

BEGIN TRY
	SELECT 
		af.[FlowName],
		afsr.[Id], afsr.[ClientApplicationId], afsr.[ApplicationFlowStepId], afsr.[IsCompleted], afsr.[StepOrder],
		afs.[StepName],afs.[ExecuteUrl]
	FROM 
		[logs].[ClientApplication] ca
	LEFT JOIN
		[logs].[ApplicationFlowStepResult] afsr ON ca.id = afsr.ClientApplicationId
	INNER JOIN
		[dbo].[ApplicationFlowStep] afs ON afsr.ApplicationFlowStepId = afs.id
	INNER JOIN
		[dbo].[ApplicationFlow] af ON afs.ApplicationFlowId = afs.ApplicationFlowId
	WHERE
		ca.[Id] = @LogId
	ORDER BY
		StepOrder ASC;

END TRY
BEGIN CATCH  


END CATCH 
GO
/****** Object:  StoredProcedure [app].[GetScoringSolutionApplication]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================================================
-- Author:		Sam Aloni
-- Create date: 11/17/2020
-- Description:	Get application to be used for Scoring Solution request
--
-- EXEC [app].[GetScoringSolutionApplication] 321818;
-- ============================================================================
CREATE PROCEDURE [app].[GetScoringSolutionApplication]

	@ApplicationID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		'MD3' AS ModelId,
		'00' AS CoBuyerCode,		-- 00 => buyer   01 => coBuyer  
		CAST(a.id AS varchar) AS ApplicationID,
		c.FirstName,
		c.LastName,
		c.MiddleName,
		'' AS CustomerSuffixTypeValue,
		c.SSN AS EncryptedSsn,
		'' AS Ssn,
		ad.HouseNumber AS HouseNumber,
		'' AS QuadRant,
		ad.StreetName AS StreetName, 
		COALESCE(st.StreetTypeName,'') AS StreetTypeName,
		ad.City AS City,
		COALESCE(s.StateAbbreviation,'') AS StateAbbreviation,
		ad.PostalCode AS PostalCode,
		FORMAT(a.DateModified,'yyyyddMM') AS DateModified,
		CAST(COALESCE(e.TotalMonths,0) AS VARCHAR(20)) AS  MonthsCurrentJob,
		CAST(COALESCE(e.PrevTotalMonths,0) AS VARCHAR(20)) AS MonthsPreviousJob,
		CAST(COALESCE(ad.TotalMonths,0) AS VARCHAR(20)) AS MonthsCurrentResidence,
		CAST(COALESCE(ad.PrevTotalMonths,0) AS VARCHAR(20)) AS MonthsPreviousResidence,
		COALESCE(ht.HousingTypeName,'') AS StateAbbreviation, 
		convert(varchar, cast(COALESCE(pc.OtherIncome,0) as money)) AS NetIncome,
		convert(varchar, cast(COALESCE(pc.PeriodPaycheck,0) as money)) AS PaymentIncome,
		convert(varchar, cast(COALESCE(pc.HousingPayment,0) as money)) AS HousingPayment,
		'' AS EquifaxRawData,
		'' ASCustomerSuffixTypeValue

	FROM application a
	INNER JOIN
		customer c ON   a.CustomerID = c.Id
	INNER JOIN
		[Address] ad ON   a.AddressID = ad.Id
	INNER JOIN
		PayingCapacity pc ON   a.PayingCapacityID = pc.Id
	LEFT JOIN
		Employment e ON  a.EmploymentID = e.Id
	LEFT JOIN
		StreetType st ON ad.StreetTypeID = st.Id
	LEFT JOIN
		[State] s ON ad.StateID = s.Id
	LEFT JOIN
		HousingType ht ON pc.HousingTypeID = ht.Id
	WHERE
		a.Id = @ApplicationID;
END
GO
/****** Object:  StoredProcedure [app].[SaveInitApp]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
------------------------------------------------------------------------------------------------------------------------
-- Date Created: Friday, November 13, 2020
-- Created By:   Sam Aloni
-- Description:	Insert new user application to all releated table
------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [app].[SaveInitApp]
	@FirstName varchar(50), 
	@MiddleName varchar(50), 
	@LastName varchar(50), 
	@HomePhone varchar(50), 
	@PhoneType varchar(50), 
	@Email varchar(50), 
	@HouseNumber varchar(50), 
	@StreetName varchar(50), 
	@StreetTypeId int, 
	@City varchar(100), 
	@Zip varchar(12), 
	@StateId int, 
	@HouseTypeId int, 
	@Referrer varchar(100), 
	@TimeAtResidenceYears varchar(100), 
	@TimeAtResidenceMonths varchar(100), 
	@TimeAtJobYears varchar(100), 
	@TimeAtJobMonths varchar(100), 
	@NetPeriodPaycheck decimal(18, 2), 
	@PaymentTypeId int, 
	@OtherIncome decimal(18, 2), 
	@OtherIncomePayPeriodId int, 
	@ActiveOrFormerMilitary bit, 
	@MilitaryChoise int, 
	@Ssn varchar(100), 
	@CurrentlyInBankruptcy bit, 
	@ClientIP varchar(100), 
	@Last4Ssn char(4) ,
	@LogId int,
	@UserId int,
	@LotID int
AS

BEGIN TRY
    
    SET NOCOUNT ON;

    BEGIN TRANSACTION

    -- type of this table's column must match the type of the
    -- identity column of the table you'll be inserting into
    DECLARE @IdentityOutput TABLE ( ID int )
	DECLARE @ExecArea varchar(20) = '';

	/*******************************************************************************************************
	** Insert into Customer table
	*******************************************************************************************************/
	SET @ExecArea = 'Insert Customer';
	DECLARE @CurrentDateTime datetime = GETDATE();
	DECLARE @CustId int = -1;
	DELETE @IdentityOutput;

    INSERT INTO [dbo].[Customer] (
        FirstName,MiddleName,LastName,SuffixTypeID,SSN,Last4SSN,DateOfBirth,DriverLicenseNumber,
		DriverLicenseStateID,HomePhone,MobilePhone,EmailAddress,MobileConsent,IsPoSubmitted,
		DateCreated,CreatedBy,DateModified,ModifiedBy,HasBeenVerified,DriverlicenseExpirationDate
    )
    OUTPUT inserted.Id INTO @IdentityOutput
    VALUES 
    (
    	@FirstName,@MiddleName,@LastName,NULL,@Ssn,@Last4Ssn,NULL,NULL,
		NULL,'',@HomePhone,@Email,1,0,
		@CurrentDateTime,@UserId,@CurrentDateTime,@UserId,0,NULL
	)
    
    SELECT @CustId = (SELECT ID FROM @IdentityOutput);

	/*******************************************************************************************************
	** Insert into Address table
	*******************************************************************************************************/
	SET @ExecArea = 'Insert Address';
	DECLARE @AddrId int = -1;
	DECLARE @aTotalMonths int;
	DELETE @IdentityOutput;

	SET @aTotalMonths = (CAST(@TimeAtResidenceYears AS INT) * 12) + CAST(@TimeAtResidenceMonths AS int);

    INSERT INTO [dbo].[Address] (
		HouseNumber,StreetName,StreetTypeID,City,StateID,PostalCode,AddressLine,TotalMonths,
		PrevStreetName,PrevCity,PrevStateID,PrevPostalCode,PrevAddressLine,PrevTotalMonths
    )
    OUTPUT inserted.Id INTO @IdentityOutput
    VALUES 
    (
    	@HouseNumber,@StreetName,@StreetTypeID,@City,@StateID,@Zip,NULL,@aTotalMonths,
		NULL,NULL,NULL,NULL,NULL,NULL
	)
    
    SELECT @AddrId = (SELECT ID FROM @IdentityOutput);

	/*******************************************************************************************************
	** Insert into PayingCapacity table
	*******************************************************************************************************/
	SET @ExecArea = 'Insert PayingCapacity';
	DECLARE @PayingCapacityId int = -1;
	DELETE @IdentityOutput;

    INSERT INTO [dbo].[PayingCapacity] (
		HousingTypeID,HousingPayment,SalaryTypeID,OtherIncome,AvailableDownPayment,Bankruptcy,
		CurrentBankruptcy,PaymentTypeID,PeriodPaycheck
    )
    OUTPUT inserted.Id INTO @IdentityOutput
    VALUES 
    (
    	@HouseTypeId,Null,@OtherIncomePayPeriodId,@OtherIncome,NULL,0,
		@CurrentlyInBankruptcy,@PaymentTypeId,@NetPeriodPaycheck
	)
    
    SELECT @PayingCapacityId = (SELECT ID FROM @IdentityOutput);

	/*******************************************************************************************************
	** Insert into Employment table
	*******************************************************************************************************/
	SET @ExecArea = 'Insert Employment';
	DECLARE @EmploymentId int = -1;
	DECLARE @eTotalMonths int;
	SET @eTotalMonths = (CAST(@TimeAtJobYears AS INT) * 12) + CAST(@TimeAtJobMonths AS int);
	DELETE @IdentityOutput;

    INSERT INTO [dbo].[Employment] (
		[Name],FullAddress,WorkPhone,Position,TotalMonths,PrevName,PrevTotalMonths
    )
    OUTPUT inserted.Id INTO @IdentityOutput
    VALUES 
    (
    	NULL,NULL,NULL,NULL,@eTotalMonths,NULL,NULL
	)
    
    SELECT @EmploymentId = (SELECT ID FROM @IdentityOutput);

    /*******************************************************************************************************
	** Insert into Application table
	*******************************************************************************************************/
	SET @ExecArea = 'Insert Application';
	DECLARE @ApplicationId int = -1;
	DELETE @IdentityOutput;
    
	IF @MilitaryChoise = 0
	BEGIN
		SET @MilitaryChoise = NULL;
	END

    INSERT INTO [dbo].[Application] (
    	[IsShortApplication],[CustomerID],[AddressID],[EmploymentID],[DateCreated],[CreatedBy],[DateModified],[ModifiedBy],[PayingCapacityID],
		[DataViewModelScoreID],[GeneralInfoID],[DealPushDataID],[ActiveOrFormerMilitary],[MilitaryChoise],[TrustScienceScoreID]
    ) 
    OUTPUT inserted.Id INTO @IdentityOutput
    VALUES 
    (
        1,@CustId,@AddrId,@EmploymentId,@CurrentDateTime,@UserId,@CurrentDateTime,@UserId,@PayingCapacityId,
		NULL,NULL,NULL,@ActiveOrFormerMilitary,@MilitaryChoise,NULL
	)
    
    SELECT @ApplicationId = (SELECT ID FROM @IdentityOutput);

    /*******************************************************************************************************
	** Insert into CreditScoreApplication table
	*******************************************************************************************************/
	SET @ExecArea = 'Insert CreditScoreApplication';
	DECLARE @CreditScoreAppId int = -1;
	DECLARE @CreditAppStatusID int = 17;   -- defualted to 'NONE'
	DELETE @IdentityOutput;

    INSERT INTO [dbo].[CreditScoreApplication] (
    	LotID,PrimaryBuyerApplicationID,CoBuyerApplicationID,CreditAppStatusID,PrimaryScoreID,CoBuyerScoreID,
		JointScoreID,IsActive,CreatedBy,ModifiedBy,DateCreated,DateModified,ClientAncestorID,SFSalesUpID,SFCreditAppID,
		ReferrerCode,ActiveOrFormerMilitary
    ) 
    OUTPUT inserted.Id INTO @IdentityOutput
    VALUES 
    (
		@LotID,@ApplicationId,NULL,@CreditAppStatusID,NULL,NULL,
		NULL,1,@UserId,@UserId,@CurrentDateTime,@CurrentDateTime,3,NULL,NULL,
		NULL,@ActiveOrFormerMilitary
	)
    
    SELECT @CreditScoreAppId = (SELECT ID FROM @IdentityOutput);

	/*******************************************************************************************************
	** save the new inserted ID's to the log table
	*******************************************************************************************************/
	SET @ExecArea = 'Update ClientApplication';
	UPDATE [logs].[ClientApplication] SET
		[CreditScoreAppId] = @CreditScoreAppId,
		[ApplicationId] = @ApplicationId,
		[PayingCapacityId] = @PayingCapacityId,
		[AddrId] = @AddrId,
		[CustId] = @CustId
	WHERE
		[Id] = @LogId;

	-- return all inserted items primary keys
	SELECT @CreditScoreAppId AS CreditScoreAppId, @ApplicationId AS ApplicationId, @PayingCapacityId AS PayingCapacityId,
		   @AddrId AS AddrId, @CustId AS CustId, @EmploymentId AS EmploymentId

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH  

    IF @@TRANCOUNT <> 0 
        BEGIN
            ROLLBACK TRANSACTION
        END;

	DECLARE @error varchar(MAX) = ' Stored procedure "'+ERROR_PROCEDURE()+'" failed.  !!!Error Message: ' + ERROR_MESSAGE() 
			+ ' !!!Error Line: ' + CAST(ERROR_LINE() AS VARCHAR(20)) + '!!!Failed Area: '+@ExecArea;

	UPDATE [logs].[ClientApplication] 
	SET
		[InitAppSaveError] = @error
	WHERE
		[Id] = @LogId;

	RAISERROR (@error ,16, -1);

END CATCH  


GO
/****** Object:  StoredProcedure [dbo].[AbandonedAppChangeItemStatus]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- Author:		Sam Aloni
-- Create date: 09/22/2020
-- Description:	Change abandoned application status after it was processed
--				and insert the datetime
--	EXEC AbandonedAppChangeItemStatus;
-- =======================================================================
CREATE PROCEDURE [dbo].[AbandonedAppChangeItemStatus] 
	@FirstName VARCHAR(500),
	@LastName VARCHAR(500),
	@ClientIP VARCHAR(500)
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE 
	  [dbo].[AbandonedApp]
	  SET [Processed] = 1, ProcessedOn = GETDATE()
	  WHERE FirstName = @FirstName AND LastName = @LastName AND ClientIP = @ClientIP AND Processed = 0;
END
GO
/****** Object:  StoredProcedure [dbo].[AbandonedAppGetItemsToProcess]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ===================================================================
-- Author:		Sam Aloni
-- Create date: 08/05/2020
-- Description:	Get the list of abandoned application to process
--
--	EXEC AbandonedAppGetItemsToProcess;
-- ===================================================================
CREATE PROCEDURE [dbo].[AbandonedAppGetItemsToProcess] 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT 
		 MAX([FirstName]) AS FirstName
		,MAX([MiddleName]) AS MiddleName
		,MAX([LastName]) AS LastName
		,MAX([PhoneNumber]) AS PhoneNumber
		,MAX([PhoneType]) AS PhoneType
		,MAX([Email]) AS Email
		,MAX([HouseNumber]) AS HouseNumber
		,MAX([ApartmentNumber]) AS ApartmentNumber
		,MAX([StreetName]) AS StreetNam
		,MAX([StreetTypeId]) AS StreetTypeId
		,MAX([City]) AS City
		,MAX([Zip]) AS Zip
		,MAX([StateId]) AS StateId
		,MAX([HouseTypeId]) AS HouseTypeId
		,MAX([ClientIP]) AS ClientIP
		,MAX([CreatedDate]) AS [CreatedDate]
	FROM AbandonedApp
	WHERE Processed = 0
	GROUP BY FirstName,LastName,ClientIP
	ORDER BY [CreatedDate] ASC;
END
GO
/****** Object:  StoredProcedure [dbo].[AbandonedAppInsert]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==================================================================
-- Author:		Sam Aloni
-- Create date: 08/04/2020
-- Description:	Insert Abandoned application to be processed later
-- ==================================================================
CREATE PROCEDURE [dbo].[AbandonedAppInsert] 
    @FirstName varchar(500)
   ,@MiddleName varchar(500)
   ,@LastName varchar(500)
   ,@PhoneNumber varchar(500)
   ,@PhoneType varchar(500)
   ,@Email varchar(500)
   ,@HouseNumber varchar(500)
   ,@ApartmentNumber varchar(500)
   ,@StreetName varchar(500)
   ,@StreetTypeId int
   ,@City varchar(500)
   ,@Zip varchar(500)
   ,@StateId int
   ,@HouseTypeId int
   ,@Referrer varchar(500)
   ,@TimeAtResidenceYears varchar(500)
   ,@TimeAtResidenceMonths varchar(500)
   ,@TimeAtJobYears varchar(500)
   ,@TimeAtJobMonths varchar(500)
   ,@NetPeriodPaycheck decimal(10,2)
   ,@PaymentTypeId int
   ,@OtherIncome decimal(10,2)
   ,@OtherIncomePayPeriodId int
   ,@ActiveOrFormerMilitary bit
   ,@Ssn varchar(500)
   ,@CurrentlyInBankruptcy bit
   ,@IsAllowTexting bit
   ,@ClientIP varchar(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO [dbo].[AbandonedApp]
				([FirstName]
				,[MiddleName]
				,[LastName]
				,[PhoneNumber]
				,[PhoneType]
				,[Email]
				,[HouseNumber]
				,[ApartmentNumber]
				,[StreetName]
				,[StreetTypeId]
				,[City]
				,[Zip]
				,[StateId]
				,[HouseTypeId]
				,[Referrer]
				,[TimeAtResidenceYears]
				,[TimeAtResidenceMonths]
				,[TimeAtJobYears]
				,[TimeAtJobMonths]
				,[NetPeriodPaycheck]
				,[PaymentTypeId]
				,[OtherIncome]
				,[OtherIncomePayPeriodId]
				,[ActiveOrFormerMilitary]
				,[Ssn]
				,[CurrentlyInBankruptcy]
				,[IsAllowTexting]
				,[ClientIP]
				)
			VALUES
				(
				@FirstName 
				,@MiddleName
				,@LastName
				,@PhoneNumber
				,@PhoneType
				,@Email
				,@HouseNumber
				,@ApartmentNumber
				,@StreetName
				,@StreetTypeId
				,@City
				,@Zip
				,@StateId
				,@HouseTypeId
				,@Referrer
				,@TimeAtResidenceYears
				,@TimeAtResidenceMonths
				,@TimeAtJobYears
				,@TimeAtJobMonths
				,@NetPeriodPaycheck
				,@PaymentTypeId
				,@OtherIncome
				,@OtherIncomePayPeriodId
				,@ActiveOrFormerMilitary
				,@Ssn
				,@CurrentlyInBankruptcy
				,@IsAllowTexting
				,COALESCE(@ClientIP,'') 
				)
END
GO
/****** Object:  StoredProcedure [dbo].[AbandonedAppSaveError]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==================================================================
-- Author:		Sam Aloni
-- Create date: 08/06/2020
-- Description:	Save Point Predictive Request and response data
--
/*
	EXEC PointPredictiveSaveReqResp 123,'reqString', 'respString', 
		'ssnString', 1,FraudEpdReportLink,DealerRiskReportLink,'123','1','22','333',1,'*********';
*/
-- ==================================================================
CREATE PROCEDURE [dbo].[AbandonedAppSaveError] 
	@AbandonedAppId int,
	@StatusCode int,
	@ReasonPhrase varchar(500),
	@Errors varchar(MAX)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[AbandonedAppError]
			   ([AbandonedAppId]
			   ,[StatusCode]
			   ,[ReasonPhrase]
			   ,[Errors]
			   ,[CreatedDate])
		 VALUES
			   (@AbandonedAppId
			   ,@StatusCode
			   ,@ReasonPhrase
			   ,@Errors
			   ,GETDATE())

END
GO
/****** Object:  StoredProcedure [dbo].[AbandonedAppWasProcessedBefore]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =======================================================================
-- Author:		Sam Aloni
-- Create date: 09/22/2020
-- Description:	Change abandoned application status after it was processed
--				and insert the datetime
--	EXEC AbandonedAppWasProcessedBefore 'Sam-1000-test','Aloni-1000-test','10.8.0.253';
-- =======================================================================
CREATE PROCEDURE [dbo].[AbandonedAppWasProcessedBefore] 
	@FirstName VARCHAR(500),
	@LastName VARCHAR(500),
	@ClientIP VARCHAR(500)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	  CASE WHEN EXISTS
	  (
		SELECT *
		  FROM [dbo].[AbandonedApp]
		  WHERE 
			FirstName = @FirstName AND 
			LastName = @LastName 
			AND ClientIP = @ClientIP 
			AND Processed = 1
	   ) 
	   THEN CAST(1 AS bit)
	   ELSE CAST(0 AS bit)
	END

END
GO
/****** Object:  StoredProcedure [dbo].[GetDealDetails]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sam Aloni
-- Create date: 07/02/2020
-- Description:	Get deal details
-- EXEC GetDealDetails 236719
-- =============================================
--SELECT top 10 * FROM [dbo].[CreditScoreApplication] 
--WHERE 
--	[PrimaryBuyerApplicationID] IS NOT NULL AND [CoBuyerApplicationID] IS NOT NULL
--ORDER BY PrimaryBuyerApplicationID DESC;

CREATE PROCEDURE [dbo].[GetDealDetails] 
	-- Add the parameters for the stored procedure here
	@id int = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	DECLARE @PrimaryBuyerApplicationID int, @CoBuyerApplicationID int, 
		@PrimaryScoreID int, @CoBuyerScoreID int, @JointScoreID int,
		@ClientAncestorID int

	SELECT 
		@PrimaryBuyerApplicationID = PrimaryBuyerApplicationID,
		@CoBuyerApplicationID = CoBuyerApplicationID,
		@PrimaryScoreID = PrimaryScoreID,
		@CoBuyerScoreID = CoBuyerScoreID,
		@JointScoreID = JointScoreID,
		@ClientAncestorID = ClientAncestorID
	FROM
		[dbo].[CreditScoreApplication]
	WHERE
		Id = @id;

	--SELECT @PrimaryBuyerApplicationID, @CoBuyerApplicationID, 
	--	@PrimaryScoreID, @CoBuyerScoreID, @JointScoreID,
	--	@ClientAncestorID 

	/******************************/

	SELECT csa.*, l.LotName AS Lot, cas.StatusName AS  CreditAppStatus,
		   ca.ClientAncestorName AS ClientAncestor,
		   u1.SPUser AS CreatedByName, u2.SPUser AS ModifiedByName,
		   FORMAT(csa.DateCreated,'MM/dd/yyyy hh:mm:s tt') AS CreatedOn
	FROM
		[dbo].[CreditScoreApplication] csa
	LEFT JOIN
		[dbo].[Lot] l ON csa.LotID = l.Id
	LEFT JOIN
		[dbo].[CreditAppStatus] cas ON csa.CreditAppStatusID = cas.Id
	LEFT JOIN 
		[dbo].[ClientAncestor] ca ON csa.ClientAncestorID = ca.Id
	LEFT JOIN
		[dbo].[User] u1 ON csa.CreatedBy = u1.Id
	LEFT JOIN
		[dbo].[User] u2 ON csa.ModifiedBy = u2.Id
	WHERE
		csa.PrimaryBuyerApplicationID = @PrimaryBuyerApplicationID;

	/******************************/

	SELECT * , m.[name] AS MilitaryEnhancementType
	FROM
		[dbo].[Application] a
	LEFT JOIN
		[dbo].[MilitaryEnhancementTypes] m ON a.MilitaryChoise = m.Id
	WHERE
		a.[Id] = @PrimaryBuyerApplicationID;

	SELECT * , m.[name] AS MilitaryEnhancementType 
	FROM
		[dbo].[Application] a
	LEFT JOIN
		[dbo].[MilitaryEnhancementTypes] m ON a.MilitaryChoise = m.Id
	WHERE
		a.[Id] = @CoBuyerApplicationID;

	/******************************/

	SELECT cust.*, s.StateName AS DriverLicenseState, st.SuffixTypeName AS Suffix,
			FORMAT(cust.DateOfBirth, 'd','us') AS DateOfBirthFormatted,
			FORMAT(DriverlicenseExpirationDate, 'd','us') AS DriverlicenseExpirationFormatted
	FROM
		[dbo].[Customer] cust
	INNER JOIN
		[dbo].[Application] app ON app.CustomerID = cust.Id
	LEFT JOIN
		[dbo].[State] s ON cust.DriverLicenseStateID = s.Id
	LEFT JOIN
		[dbo].[SuffixType] st ON cust.SuffixTypeID = st.Id
	WHERE
		app.[Id] = @PrimaryBuyerApplicationID;

	SELECT cust.*, s.StateName AS DriverLicenseState, st.SuffixTypeName AS Suffix ,
			FORMAT(cust.DateOfBirth, 'd','us') AS DateOfBirthFormatted, 
			FORMAT(DriverlicenseExpirationDate, 'd','us') AS DriverlicenseExpirationFormatted
	FROM
		[dbo].[Customer] cust
	INNER JOIN
		[dbo].[Application] app ON app.CustomerID = cust.Id
	LEFT JOIN
		[dbo].[State] s ON cust.DriverLicenseStateID = s.Id
	LEFT JOIN
		[dbo].[SuffixType] st ON cust.SuffixTypeID = st.Id
	WHERE
		app.[Id] = @CoBuyerApplicationID;

	/******************************/

	SELECT ad.*, s.StateName AS StateName, st.StreetTypeName
	FROM
		[dbo].[Application] app
	INNER JOIN
		[dbo].[Address] ad ON app.AddressID = ad.Id
	LEFT JOIN
		[dbo].[State] s ON ad.StateID = s.Id
	LEFT JOIN
		[dbo].[StreetType] st ON ad.StreetTypeID = st.Id
	WHERE
		app.[Id] = @PrimaryBuyerApplicationID;

	SELECT ad.*, s.StateName AS StateName, st.StreetTypeName 
	FROM
		[dbo].[Application] app
	INNER JOIN
		[dbo].[Address] ad ON app.AddressID = ad.Id
	LEFT JOIN
		[dbo].[State] s ON ad.StateID = s.Id
	LEFT JOIN
		[dbo].[StreetType] st ON ad.StreetTypeID = st.Id
	WHERE
		app.[Id] = @CoBuyerApplicationID;

	/******************************/

	SELECT dpd.* 
	FROM
		[dbo].[Application] app
	INNER JOIN
		[dbo].[DealPushData] dpd ON app.DealPushDataID = dpd.Id
	WHERE
		app.[Id] = @PrimaryBuyerApplicationID;

	SELECT dpd.* 
	FROM
		[dbo].[Application] app
	INNER JOIN
		[dbo].[DealPushData] dpd ON app.DealPushDataID = dpd.Id
	WHERE
		app.[Id] = @CoBuyerApplicationID;

	/******************************/

	SELECT e.* 
	FROM
		[dbo].[Application] app
	INNER JOIN
		[dbo].[Employment] e ON app.EmploymentID = e.id
	WHERE
		app.[Id] = @PrimaryBuyerApplicationID;

	SELECT e.* 
	FROM
		[dbo].[Application] app
	INNER JOIN
		[dbo].[Employment] e ON app.EmploymentID = e.id
	WHERE
		app.[Id] = @CoBuyerApplicationID;

	/******************************/

	SELECT pc.*, ht.HousingTypeName AS HousingType,
			st.SalaryLabel AS SalaryType,
			pt.PaymentLabel AS PaymentType
	FROM
		[dbo].[Application] app
	INNER JOIN
		PayingCapacity pc ON app.PayingCapacityID = pc.id
	LEFT JOIN
		HousingType ht ON pc.HousingTypeID = ht.id
	LEFT JOIN
		SalaryType st ON pc.SalaryTypeID = st.id
	LEFT JOIN
		PaymentType pt ON pc.PaymentTypeID = pt.Id

	WHERE
		app.[Id] = @PrimaryBuyerApplicationID;

	SELECT pc.* 
	FROM
		[dbo].[Application] app
	INNER JOIN
		PayingCapacity pc ON app.PayingCapacityID = pc.id
	WHERE
		app.[Id] = @CoBuyerApplicationID;

	/******************************/

	SELECT gi.* ,
			ppo.PriorPurchaseOptionName AS PriorPurchase, 
			rro.RepeatReasonOptionName AS RepeatReason,  
			sp.SalesPerson AS SalesPeople
	FROM
		[dbo].[Application] app
	INNER JOIN
		GeneralInfo gi ON app.GeneralInfoID = gi.Id
	LEFT JOIN
		PriorPurchaseOption ppo ON gi.PriorPurchaseOptionID = ppo.Id
	LEFT JOIN
		RepeatReasonOption rro ON gi.RepeatReasonOptionID = rro.Id
	LEFT JOIN
		SalesPeople sp ON gi.SalesPeopleID = sp.Id
	WHERE
		app.[Id] = @PrimaryBuyerApplicationID;

	SELECT gi.* ,
			ppo.PriorPurchaseOptionName AS PriorPurchase, 
			rro.RepeatReasonOptionName AS RepeatReason,  
			sp.SalesPerson AS SalesPeople 
	FROM
		[dbo].[Application] app
	INNER JOIN
		GeneralInfo gi ON app.GeneralInfoID = gi.Id
	LEFT JOIN
		PriorPurchaseOption ppo ON gi.PriorPurchaseOptionID = ppo.Id
	LEFT JOIN
		RepeatReasonOption rro ON gi.RepeatReasonOptionID = rro.Id
	LEFT JOIN
		SalesPeople sp ON gi.SalesPeopleID = sp.Id
	WHERE
		app.[Id] = @CoBuyerApplicationID;

	/******************************/

	SELECT 
		c.*
	FROM
		[dbo].[Comment] c
	WHERE
		c.Id = @id;

	/******************************/

	SELECT s.*
	FROM
		[dbo].[Score] s
	WHERE
		s.[Id] = @PrimaryScoreID

	SELECT s.*
	FROM
		[dbo].[Score] s
	WHERE
		s.[Id] = @CoBuyerScoreID

	/******************************/

	SELECT gss.*
	FROM
		[dbo].[Score] s
	INNER JOIN
		[dbo].[GradeScoreSegment] gss ON s.GradeScoreSegmentID = gss.Id
	WHERE
		s.[Id] = @PrimaryScoreID;

	SELECT gss.*
	FROM
		[dbo].[Score] s
	INNER JOIN
		[dbo].[GradeScoreSegment] gss ON s.GradeScoreSegmentID = gss.Id
	WHERE
		s.[Id] = @CoBuyerScoreID;

	/******************************/
	SELECT g.*
	FROM
		[dbo].[Score] s
	INNER JOIN
		[dbo].[GradeScoreSegment] gss ON s.GradeScoreSegmentID = gss.Id
	INNER JOIN
		[dbo].[Grading] g ON gss.id = g.GradeScoreSegmentID
	WHERE
		s.[Id] = @PrimaryScoreID;

	SELECT g.*
	FROM
		[dbo].[Score] s
	INNER JOIN
		[dbo].[GradeScoreSegment] gss ON s.GradeScoreSegmentID = gss.Id
	INNER JOIN
		[dbo].[Grading] g ON gss.id = g.GradeScoreSegmentID
	WHERE
		s.[Id] = @CoBuyerScoreID;
	/******************************/

END
GO
/****** Object:  StoredProcedure [dbo].[InsertApplicationClientIP]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ===================================================
-- Author:		Sam Aloni
-- Create date: 10/02/2020
-- Description:	Insert online application client IP
-- ===================================================
CREATE PROCEDURE [dbo].[InsertApplicationClientIP]
		@CreditScoreApplicationID int, @CustomerID int, @clientIP varchar(500)
AS
INSERT INTO [dbo].[ApplicationClientIP]
           ([CreditScoreApplicationID]
           ,[CustomerID]
           ,[ClientIP])
     VALUES
           (
		    @CreditScoreApplicationID
		   ,@CustomerID
		   ,@clientIP
		   )

GO
/****** Object:  StoredProcedure [dbo].[ManageInventory]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ManageInventory]
    
AS
BEGIN

    
INSERT INTO [dbo].[VehicleInventory]
           ([Stockno]
           ,[CarMake]
           ,[CarModel]
           ,[Mileage]
           ,[CarYear]
           ,[SalesPrice]
           ,[LotID]
           ,[FuelType]
           ,[CarColor]
           ,[Vin]
           ,[Transmission]
           ,[EngineCycles]
           ,[OriginalCarTypeID]
           ,[DaysInInven]
           ,[IsSellingEnabled])
   select I.[Stockno]
           ,I.[CarMake]
           ,I.[CarModel]
           ,I.[Mileage]
           ,I.[CarYear]
           ,I.[SalesPrice]
           ,I.[LotID]
           ,I.[FuelType]
           ,substring(I.[CarColor],1,12) as Carcolor
           ,I.[Vin]
           ,I.[Transmission]
           ,I.[EngineCycles]
           ,I.[OriginalCarTypeID]
           ,I.[DaysInInven]
           ,I.[IsSellingEnabled] 
	from  [dbo].[vw_Inven] as I
	LEFT JOIN [dbo].[VehicleInventory] as v2
		ON v2.[Stockno] = I.[Stockno]
    WHERE v2.id IS NULL and I.lotid<>'98'
	
--remove sold vehicles
update VehicleInventory
set IsSellingEnabled='0'
 from VehicleInventory i, MERCEDES.bridge.dbo.BookedDeals b
where i.Stockno=b.stockno and i.IsSellingEnabled='1'

--Update older vehicles
update VehicleInventory
set IsSellingEnabled='1' from 
VehicleInventory v, shelby.shelby_idms.dbo.inven i  
where v.Stockno=i.stockno and v.IsSellingEnabled='0'

--Update days in Inventory
update VehicleInventory
set DaysInInven=i.DaysInInven from [dbo].[vw_Inven] i, VehicleInventory v
where i.Stockno=v.Stockno and i.DaysInInven<>v.DaysInInven and i.IsSellingEnabled='1'

--Update lot
update VehicleInventory
set LotID=i.LotID from [dbo].[vw_Inven] i, VehicleInventory v
where i.Stockno=v.Stockno and i.LotID<>v.LotID and i.IsSellingEnabled='1'
and i.lotid<>'98'


--Update Price
update VehicleInventory
set SalesPrice=i.SalesPrice  from [dbo].[vw_Inven] i, VehicleInventory v
where i.Stockno=v.Stockno and i.salesprice<>v.salesprice and i.IsSellingEnabled='1'


END
GO
/****** Object:  StoredProcedure [dbo].[PointPredictiveGetApplication]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Travis / Sam
-- Create date: 08/01/2020
-- Description:	Get application data for Point Predictive
--
--  08/07/2020 - remove spaces from phone numbers
-- EXEC PointPredictiveGetApplication 281473;  --236720;-- 236622; -- 236720;  --236627;  -- 236720  -- 248804
-- EXEC PointPredictiveGetApplication 236627;
-- collect credit with status = "PendingPaycall"
-- select top 100 * from creditscoreapplication where CreditAppStatusID IN ( 8, 9) order by id desc;
-- =============================================
CREATE PROCEDURE [dbo].[PointPredictiveGetApplication]
	-- Add the parameters for the stored procedure here
	@creditID int
AS

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


SELECT 
	'2018.05' as InterfaceVersion,
	'USAUTO' as AccountIdentifier,
	'' as LenderIdentifier,
	ca.id as ApplicationIdentifier,
	convert(varchar(8),ca.datecreated,112) as ApplicationDate,
	'PENDING' as ApplicationStatus,
	lt.lotcode as	DealerIdentifier,

	upper(cu.firstname) as FirstName,
	upper(cu.lastname) as LastName,
	upper(ad.housenumber+ ' '+ad.streetname + ' '+st.streettypename) as StreetAddress,
	upper(ad.city) as City,
	upper(sta.Stateabbreviation) as State,
	ad.postalcode as Zip,
	'USA' as Country,
	isNull(replace(replace(replace(replace(Replace(cu.homephone,'(',''),')',''),'-',''),'.',''),' ','') ,'') as HomePhoneNumber,
	isNull( replace(replace(replace(replace(Replace(em.workphone,'(',''),')',''),'-',''),'.',''),' ','')  ,'')  as WorkPhoneNumber,
	isNull( replace(replace(replace(replace(Replace(cu.mobilephone,'(',''),')',''),'-',''),'.',''),' ',''),'') as CellPhoneNumber,
	isNull(upper(cu.emailaddress),'') as [Email],
	isNull(convert(varchar(8),cu.dateofbirth,112),'') as DateofBirth,
	 (cu.ssn)  as SSN,
	case when pc.housingtypeid='2' then 'O'
	else 'R'
	END AS RentOwn,
	'' as RentMortgage,
	ad.totalmonths as MonthsatResidence,
	isNull(upper(em.position),'') as Occupation,
	case pc.paymenttypeid
	when '1' then pc.periodpaycheck * 52
	when '2' then pc.periodpaycheck * 26
	when '3' then pc.periodpaycheck * 24
	else pc.periodpaycheck * 12
	end as AnnualIncome,
	'' as [SelfEmployed],
	isNull(upper(em.name),'') as EmployerName, 
	isNull(upper(em.fulladdress),'') as EmployerStreetAddress,
	'' as  EmployerCity,
	'' as EmployerState,
	'' as EmployerZIP,
	isNull(replace(replace(replace(replace(Replace(em.workphone,'(',''),')',''),'-',''),'.',''),' ',''),'')  as EmployerPhone,
	em.totalmonths as MonthsatEmployer,
	'0' as OtherBankRelationships,
	'' as CustomerSinceDate,
	case ca.clientancestorid
	when '3' then 'PREQUAL'
	WHEN '4' THEN 'PHONE'
	ELSE 'STORE'
	end as Channel ,
	--End PrimaryBorrower Info

	--Begin Loan Information
	sc.MaxFinance as LoanAmount,
	(sc.maxpurchaseprice*sc.mindownpaymentpercent) as TotalDownPayment,
	sc.availabledownpayment as CashDownPayment,
	'60' as Term,
	sc.pti*100 as [PaymentToIncomeRatio],
	--End Loan Information

	--Begin Credit Information PrimaryBorrower
	dv.beaconscore as CreditScore,
	'' as TimeinFile,
	'' as DebtToIncomeRatio,
	case when isNull(dv.INQ6_5,'')<0 then 0 else isNull(dv.INQ6_5,'') end as NumberofCreditInquiriesinprevioustwoweeks, 
	isNull(dv.HCACC_3,'') as HighestCreditLimitfromTradesinGoodStanding,
	isNull(isNull(dv.OPNSAT1,0)/nullif(dv.satrat1,0),0) as TotalNumberofTradeLines,

	case when (isNull(dv.OPNSAT1,0) + isNUll(dv.ACT6_1,0)) <0 then 0 else (isNull(dv.OPNSAT1,0) + isNUll(dv.ACT6_1,0)) end as NumberofOpenTradeLines,
	case when isNull(dv.pstdu_13,0) <0 then 0 else isNull(dv.pstdu_13,0)  end as NumberofPositiveAutoTrades	,
	case when isNull(dv.TSAT_11,'') <0 then 0 else isNull(dv.TSAT_11,'') end as NumberofMortgageTradeLines	,
	null as NumberofAuthorizedTradeLines	,
	case when DV.OLDAG_1>0 then convert(varchar(8),dateadd(MONTH,DV.OLDAG_1*-1, DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0)),112)
	else '' end as DateofOldestTradeLine,
	sc.MaxPurchasePrice as SalePrice,
	'' as YearofManufacture,
	'' as Make,
	'' as Model	,
	'' as VIN	,
	'USED' as NeworUsed,
	'' as Mileage,


	isNull(upper(cuc.firstname),'') as CobFirstName,
	isNull(upper(cuc.lastname),'') as CobLastName,
	isNull(upper(cad.housenumber+ ' '+cad.streetname + ' '+cst.streettypename),'') as CobStreetAddress,
	isNull(upper(cad.city),'') as CobCity,
	isNull(upper(csta.Stateabbreviation),'') as CobState,
	isNull(cad.postalcode,'') as CobZip,
	isNull(replace(replace(replace(Replace(cuc.homephone,'(',''),')',''),'-',''),'.',''),'') as CobHomePhoneNumber,
	isNull(replace(replace(replace(Replace(cem.workphone,'(',''),')',''),'-',''),'.',''),'')  as CobWorkPhoneNumber,
	isNull(replace(replace(replace(Replace(cuc.mobilephone,'(',''),')',''),'-',''),'.',''),'') as CobCellPhoneNumber,
	isNull(upper(cuc.emailaddress),'') as [CobEmail],
	isNull(convert(varchar(8),cuc.dateofbirth,112),'') as CobDateofBirth,
	isNull(
	(
		cast(
				(case cpc.paymenttypeid
				when '1' then cpc.periodpaycheck * 52
				when '2' then cpc.periodpaycheck * 26
				when '3' then cpc.periodpaycheck * 24
				else cpc.periodpaycheck * 12
				end) as varchar(10))),'') as CobAnnualIncome,
	'' as CobRelationship,
	isNull(cdv.beaconscore,'') as CobCreditScore,
	isNull(substring(convert(varchar(8),cu.dateofbirth,112),1,4),'') as PrimaryBorrowerYearofBirth,
	isNull(substring(convert(varchar(8),cuc.dateofbirth,112),1,4),'')  as CoBorrowerYearofBirth,
	'' as PrimaryBorrowerCreditScoreRange,
	'' as CoBorrowerCreditScoreRange,
	case when dv.satrat1 <0 then 0 else dv.satrat1 end  as UDF1,
	case when dv.mthdel1 <0 then 0 else dv.mthdel1 end as UDF2,
	case when dv.o24bl_6 <0 then 0 else dv.o24bl_6 end as UDF3,
	case when dv.plus90  <0 then 0 else dv.plus90 end as UDF4,
	case when dv.pstdu_1 <0 then 0 else dv.pstdu_1 end as UDF5,
	case when dv.ratio_1 <0 then 0 else dv.ratio_1 end as UDF6

FROM dbo.creditscoreapplication ca with (nolock)
	inner join
	dbo.lot lt with (nolock) on ca.lotid=lt.id
	inner join
	dbo.application a with (nolock) on ca.primarybuyerapplicationid=a.id
	left join
	dbo.application coa with (nolock) on ca.cobuyerapplicationid=coa.id
	inner join
	dbo.score sc with (nolock) on ca.primaryscoreid=sc.id
	left join
	dbo.score csc with (nolock) on ca.CoBuyerscoreid=csc.id
	inner join
	dbo.dataviewmodelscore dv with (nolock) on a.dataviewmodelscoreid=dv.id
	left join
	dbo.dataviewmodelscore cdv with (nolock) on coa.dataviewmodelscoreid=cdv.id
	inner join
	dbo.customer cu with (nolock)  on a.customerid=cu.id
	left join
	dbo.customer cuc with (nolock) on coa.customerid=cuc.id
	inner join
	dbo.address ad with (nolock) on a.addressid=ad.id
	left join
	dbo.address cad with (nolock) on coa.addressid=cad.id
	inner join
	dbo.streettype st with (nolock) on ad.streettypeid=st.id
	left join
	dbo.streettype cst with (nolock) on cad.streettypeid=cst.id
	inner join
	dbo.state sta with (nolock) on ad.stateid=sta.id
	left join
	dbo.state csta with (nolock) on cad.stateid=csta.id
	inner join
	dbo.PayingCapacity pc with (nolock) on a.payingcapacityid=pc.id
	left join
	dbo.PayingCapacity cpc with (nolock) on coa.payingcapacityid=cpc.id
	inner join
	dbo.Employment em with (nolock) on a.employmentid=em.id
	left join
	dbo.Employment cem with (nolock) on coa.employmentid=cem.id
WHERE 
	ca.id=@creditID AND ca.CreditAppStatusID>7




END
GO
/****** Object:  StoredProcedure [dbo].[PointPredictiveGetApplicationIgnoreStatus]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Travis / Sam
-- Create date: 08/27/2020
-- Description:	Get application data for Point Predictive Ignor status (For Auto Score Process)
--
--  08/07/2020 - remove spaces from phone numbers
-- EXEC PointPredictiveGetApplicationIgnoreStatus 281473;  --236720;-- 236622; -- 236720;  --236627;  -- 236720  -- 248804
-- EXEC PointPredictiveGetApplicationIgnoreStatus 236627;
-- collect credit with status = "PendingPaycall"
-- select top 100 * from creditscoreapplication where CreditAppStatusID IN ( 8, 9) order by id desc;
-- =============================================
CREATE PROCEDURE [dbo].[PointPredictiveGetApplicationIgnoreStatus]
	-- Add the parameters for the stored procedure here
	@creditID int
AS

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


SELECT 
	'2018.05' as InterfaceVersion,
	'USAUTO' as AccountIdentifier,
	'' as LenderIdentifier,
	ca.id as ApplicationIdentifier,
	convert(varchar(8),ca.datecreated,112) as ApplicationDate,
	'PENDING' as ApplicationStatus,
	lt.lotcode as	DealerIdentifier,

	upper(cu.firstname) as FirstName,
	upper(cu.lastname) as LastName,
	upper(ad.housenumber+ ' '+ad.streetname + ' '+st.streettypename) as StreetAddress,
	upper(ad.city) as City,
	upper(sta.Stateabbreviation) as State,
	ad.postalcode as Zip,
	'USA' as Country,
	isNull(replace(replace(replace(replace(Replace(cu.homephone,'(',''),')',''),'-',''),'.',''),' ','') ,'') as HomePhoneNumber,
	isNull( replace(replace(replace(replace(Replace(em.workphone,'(',''),')',''),'-',''),'.',''),' ','')  ,'')  as WorkPhoneNumber,
	isNull( replace(replace(replace(replace(Replace(cu.mobilephone,'(',''),')',''),'-',''),'.',''),' ',''),'') as CellPhoneNumber,
	isNull(upper(cu.emailaddress),'') as [Email],
	isNull(convert(varchar(8),cu.dateofbirth,112),'') as DateofBirth,
	 (cu.ssn)  as SSN,
	case when pc.housingtypeid='2' then 'O'
	else 'R'
	END AS RentOwn,
	'' as RentMortgage,
	ad.totalmonths as MonthsatResidence,
	isNull(upper(em.position),'') as Occupation,
	case pc.paymenttypeid
	when '1' then pc.periodpaycheck * 52
	when '2' then pc.periodpaycheck * 26
	when '3' then pc.periodpaycheck * 24
	else pc.periodpaycheck * 12
	end as AnnualIncome,
	'' as [SelfEmployed],
	isNull(upper(em.name),'') as EmployerName, 
	isNull(upper(em.fulladdress),'') as EmployerStreetAddress,
	'' as  EmployerCity,
	'' as EmployerState,
	'' as EmployerZIP,
	isNull(replace(replace(replace(replace(Replace(em.workphone,'(',''),')',''),'-',''),'.',''),' ',''),'')  as EmployerPhone,
	em.totalmonths as MonthsatEmployer,
	'0' as OtherBankRelationships,
	'' as CustomerSinceDate,
	case ca.clientancestorid
	when '3' then 'PREQUAL'
	WHEN '4' THEN 'PHONE'
	ELSE 'STORE'
	end as Channel ,
	--End PrimaryBorrower Info

	--Begin Loan Information
	sc.MaxFinance as LoanAmount,
	(sc.maxpurchaseprice*sc.mindownpaymentpercent) as TotalDownPayment,
	sc.availabledownpayment as CashDownPayment,
	'60' as Term,
	sc.pti*100 as [PaymentToIncomeRatio],
	--End Loan Information

	--Begin Credit Information PrimaryBorrower
	dv.beaconscore as CreditScore,
	'' as TimeinFile,
	'' as DebtToIncomeRatio,
	case when isNull(dv.INQ6_5,'')<0 then 0 else isNull(dv.INQ6_5,'') end as NumberofCreditInquiriesinprevioustwoweeks, 
	isNull(dv.HCACC_3,'') as HighestCreditLimitfromTradesinGoodStanding,
	isNull(isNull(dv.OPNSAT1,0)/nullif(dv.satrat1,0),0) as TotalNumberofTradeLines,

	case when (isNull(dv.OPNSAT1,0) + isNUll(dv.ACT6_1,0)) <0 then 0 else (isNull(dv.OPNSAT1,0) + isNUll(dv.ACT6_1,0)) end as NumberofOpenTradeLines,
	case when isNull(dv.pstdu_13,0) <0 then 0 else isNull(dv.pstdu_13,0)  end as NumberofPositiveAutoTrades	,
	case when isNull(dv.TSAT_11,'') <0 then 0 else isNull(dv.TSAT_11,'') end as NumberofMortgageTradeLines	,
	null as NumberofAuthorizedTradeLines	,
	case when DV.OLDAG_1>0 then convert(varchar(8),dateadd(MONTH,DV.OLDAG_1*-1, DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0)),112)
	else '' end as DateofOldestTradeLine,
	sc.MaxPurchasePrice as SalePrice,
	'' as YearofManufacture,
	'' as Make,
	'' as Model	,
	'' as VIN	,
	'USED' as NeworUsed,
	'' as Mileage,


	isNull(upper(cuc.firstname),'') as CobFirstName,
	isNull(upper(cuc.lastname),'') as CobLastName,
	isNull(upper(cad.housenumber+ ' '+cad.streetname + ' '+cst.streettypename),'') as CobStreetAddress,
	isNull(upper(cad.city),'') as CobCity,
	isNull(upper(csta.Stateabbreviation),'') as CobState,
	isNull(cad.postalcode,'') as CobZip,
	isNull(replace(replace(replace(Replace(cuc.homephone,'(',''),')',''),'-',''),'.',''),'') as CobHomePhoneNumber,
	isNull(replace(replace(replace(Replace(cem.workphone,'(',''),')',''),'-',''),'.',''),'')  as CobWorkPhoneNumber,
	isNull(replace(replace(replace(Replace(cuc.mobilephone,'(',''),')',''),'-',''),'.',''),'') as CobCellPhoneNumber,
	isNull(upper(cuc.emailaddress),'') as [CobEmail],
	isNull(convert(varchar(8),cuc.dateofbirth,112),'') as CobDateofBirth,
	isNull(
	(
		cast(
				(case cpc.paymenttypeid
				when '1' then cpc.periodpaycheck * 52
				when '2' then cpc.periodpaycheck * 26
				when '3' then cpc.periodpaycheck * 24
				else cpc.periodpaycheck * 12
				end) as varchar(10))),'') as CobAnnualIncome,
	'' as CobRelationship,
	isNull(cdv.beaconscore,'') as CobCreditScore,
	isNull(substring(convert(varchar(8),cu.dateofbirth,112),1,4),'') as PrimaryBorrowerYearofBirth,
	isNull(substring(convert(varchar(8),cuc.dateofbirth,112),1,4),'')  as CoBorrowerYearofBirth,
	'' as PrimaryBorrowerCreditScoreRange,
	'' as CoBorrowerCreditScoreRange,
	case when dv.satrat1 <0 then 0 else dv.satrat1 end  as UDF1,
	case when dv.mthdel1 <0 then 0 else dv.mthdel1 end as UDF2,
	case when dv.o24bl_6 <0 then 0 else dv.o24bl_6 end as UDF3,
	case when dv.plus90  <0 then 0 else dv.plus90 end as UDF4,
	case when dv.pstdu_1 <0 then 0 else dv.pstdu_1 end as UDF5,
	case when dv.ratio_1 <0 then 0 else dv.ratio_1 end as UDF6

FROM dbo.creditscoreapplication ca with (nolock)
	inner join
	dbo.lot lt with (nolock) on ca.lotid=lt.id
	inner join
	dbo.application a with (nolock) on ca.primarybuyerapplicationid=a.id
	left join
	dbo.application coa with (nolock) on ca.cobuyerapplicationid=coa.id
	inner join
	dbo.score sc with (nolock) on ca.primaryscoreid=sc.id
	left join
	dbo.score csc with (nolock) on ca.CoBuyerscoreid=csc.id
	inner join
	dbo.dataviewmodelscore dv with (nolock) on a.dataviewmodelscoreid=dv.id
	left join
	dbo.dataviewmodelscore cdv with (nolock) on coa.dataviewmodelscoreid=cdv.id
	inner join
	dbo.customer cu with (nolock)  on a.customerid=cu.id
	left join
	dbo.customer cuc with (nolock) on coa.customerid=cuc.id
	inner join
	dbo.address ad with (nolock) on a.addressid=ad.id
	left join
	dbo.address cad with (nolock) on coa.addressid=cad.id
	inner join
	dbo.streettype st with (nolock) on ad.streettypeid=st.id
	left join
	dbo.streettype cst with (nolock) on cad.streettypeid=cst.id
	inner join
	dbo.state sta with (nolock) on ad.stateid=sta.id
	left join
	dbo.state csta with (nolock) on cad.stateid=csta.id
	inner join
	dbo.PayingCapacity pc with (nolock) on a.payingcapacityid=pc.id
	left join
	dbo.PayingCapacity cpc with (nolock) on coa.payingcapacityid=cpc.id
	inner join
	dbo.Employment em with (nolock) on a.employmentid=em.id
	left join
	dbo.Employment cem with (nolock) on coa.employmentid=cem.id
WHERE 
	ca.id=@creditID;




END
GO
/****** Object:  StoredProcedure [dbo].[PointPredictiveGetApplicationNew]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Travis / Sam
-- Create date: 08/01/2020
-- Description:	Get application data for Point Predictive
--
-- EXEC PointPredictiveGetApplication 281473;  --236720;-- 236622; -- 236720;  --236627;  -- 236720  -- 248804
-- EXEC PointPredictiveGetApplication 236627;
-- collect credit with status = "PendingPaycall"
-- select top 100 * from creditscoreapplication where CreditAppStatusID IN ( 8, 9) order by id desc;
-- =============================================
create PROCEDURE [dbo].[PointPredictiveGetApplicationNew] 
	-- Add the parameters for the stored procedure here
	@creditID int
AS

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


SELECT 
	'2018.05' as InterfaceVersion,
	'USAUTO' as AccountIdentifier,
	'' as LenderIdentifier,
	ca.id as ApplicationIdentifier,
	convert(varchar(8),ca.datecreated,112) as ApplicationDate,
	'PENDING' as ApplicationStatus,
	lt.lotcode as	DealerIdentifier,

	upper(cu.firstname) as FirstName,
	upper(cu.lastname) as LastName,
	upper(ad.housenumber+ ' '+ad.streetname + ' '+st.streettypename) as StreetAddress,
	upper(ad.city) as City,
	upper(sta.Stateabbreviation) as State,
	ad.postalcode as Zip,
	'USA' as Country,
	isNull(replace(replace(replace(replace(Replace(cu.homephone,'(',''),')',''),'-',''),'.',''),' ','') ,'') as HomePhoneNumber,
	isNull( replace(replace(replace(replace(Replace(em.workphone,'(',''),')',''),'-',''),'.',''),' ','')  ,'')  as WorkPhoneNumber,
	isNull( replace(replace(replace(replace(Replace(cu.mobilephone,'(',''),')',''),'-',''),'.',''),' ',''),'') as CellPhoneNumber,
	isNull(upper(cu.emailaddress),'') as [Email],
	isNull(convert(varchar(8),cu.dateofbirth,112),'') as DateofBirth,
	 (cu.ssn)  as SSN,
	case when pc.housingtypeid='2' then 'O'
	else 'R'
	END AS RentOwn,
	'' as RentMortgage,
	ad.totalmonths as MonthsatResidence,
	isNull(upper(em.position),'') as Occupation,
	case pc.paymenttypeid
	when '1' then pc.periodpaycheck * 52
	when '2' then pc.periodpaycheck * 26
	when '3' then pc.periodpaycheck * 24
	else pc.periodpaycheck * 12
	end as AnnualIncome,
	'' as [SelfEmployed],
	isNull(upper(em.name),'') as EmployerName, 
	isNull(upper(em.fulladdress),'') as EmployerStreetAddress,
	'' as  EmployerCity,
	'' as EmployerState,
	'' as EmployerZIP,
	isNull(replace(replace(replace(Replace(em.workphone,'(',''),')',''),'-',''),'.',''),'')  as EmployerPhone,
	em.totalmonths as MonthsatEmployer,
	'0' as OtherBankRelationships,
	'' as CustomerSinceDate,
	case ca.clientancestorid
	when '3' then 'PREQUAL'
	WHEN '4' THEN 'PHONE'
	ELSE 'STORE'
	end as Channel ,
	--End PrimaryBorrower Info

	--Begin Loan Information
	sc.MaxFinance as LoanAmount,
	(sc.maxpurchaseprice*sc.mindownpaymentpercent) as TotalDownPayment,
	sc.availabledownpayment as CashDownPayment,
	'60' as Term,
	sc.pti*100 as [PaymentToIncomeRatio],
	--End Loan Information

	--Begin Credit Information PrimaryBorrower
	dv.beaconscore as CreditScore,
	'' as TimeinFile,
	'' as DebtToIncomeRatio,
	case when isNull(dv.INQ6_5,'')<0 then 0 else isNull(dv.INQ6_5,'') end as NumberofCreditInquiriesinprevioustwoweeks, 
	isNull(dv.HCACC_3,'') as HighestCreditLimitfromTradesinGoodStanding,
	isNull(isNull(dv.OPNSAT1,0)/nullif(dv.satrat1,0),0) as TotalNumberofTradeLines,

	case when (isNull(dv.OPNSAT1,0) + isNUll(dv.ACT6_1,0)) <0 then 0 else (isNull(dv.OPNSAT1,0) + isNUll(dv.ACT6_1,0)) end as NumberofOpenTradeLines,
	case when isNull(dv.pstdu_13,0) <0 then 0 else isNull(dv.pstdu_13,0)  end as NumberofPositiveAutoTrades	,
	case when isNull(dv.TSAT_11,'') <0 then 0 else isNull(dv.TSAT_11,'') end as NumberofMortgageTradeLines	,
	null as NumberofAuthorizedTradeLines	,
	case when DV.OLDAG_1>0 then convert(varchar(8),dateadd(MONTH,DV.OLDAG_1*-1, DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0)),112)
	else '' end as DateofOldestTradeLine,
	sc.MaxPurchasePrice as SalePrice,
	'' as YearofManufacture,
	'' as Make,
	'' as Model	,
	'' as VIN	,
	'USED' as NeworUsed,
	'' as Mileage,


	isNull(upper(cuc.firstname),'') as CobFirstName,
	isNull(upper(cuc.lastname),'') as CobLastName,
	isNull(upper(cad.housenumber+ ' '+cad.streetname + ' '+cst.streettypename),'') as CobStreetAddress,
	isNull(upper(cad.city),'') as CobCity,
	isNull(upper(csta.Stateabbreviation),'') as CobState,
	isNull(cad.postalcode,'') as CobZip,
	isNull(replace(replace(replace(Replace(cuc.homephone,'(',''),')',''),'-',''),'.',''),'') as CobHomePhoneNumber,
	isNull(replace(replace(replace(Replace(cem.workphone,'(',''),')',''),'-',''),'.',''),'')  as CobWorkPhoneNumber,
	isNull(replace(replace(replace(Replace(cuc.mobilephone,'(',''),')',''),'-',''),'.',''),'') as CobCellPhoneNumber,
	isNull(upper(cuc.emailaddress),'') as [CobEmail],
	isNull(convert(varchar(8),cuc.dateofbirth,112),'') as CobDateofBirth,
	isNull(
	(
		cast(
				(case cpc.paymenttypeid
				when '1' then cpc.periodpaycheck * 52
				when '2' then cpc.periodpaycheck * 26
				when '3' then cpc.periodpaycheck * 24
				else cpc.periodpaycheck * 12
				end) as varchar(10))),'') as CobAnnualIncome,
	'' as CobRelationship,
	isNull(cdv.beaconscore,'') as CobCreditScore,
	isNull(substring(convert(varchar(8),cu.dateofbirth,112),1,4),'') as PrimaryBorrowerYearofBirth,
	isNull(substring(convert(varchar(8),cuc.dateofbirth,112),1,4),'')  as CoBorrowerYearofBirth,
	'' as PrimaryBorrowerCreditScoreRange,
	'' as CoBorrowerCreditScoreRange,
	case when dv.satrat1 <0 then 0 else dv.satrat1 end  as UDF1,
	case when dv.mthdel1 <0 then 0 else dv.mthdel1 end as UDF2,
	case when dv.o24bl_6 <0 then 0 else dv.o24bl_6 end as UDF3,
	case when dv.plus90  <0 then 0 else dv.plus90 end as UDF4,
	case when dv.pstdu_1 <0 then 0 else dv.pstdu_1 end as UDF5,
	case when dv.ratio_1 <0 then 0 else dv.ratio_1 end as UDF6

FROM dbo.creditscoreapplication ca with (nolock)
	inner join
	dbo.lot lt with (nolock) on ca.lotid=lt.id
	inner join
	dbo.application a with (nolock) on ca.primarybuyerapplicationid=a.id
	left join
	dbo.application coa with (nolock) on ca.cobuyerapplicationid=coa.id
	inner join
	dbo.score sc with (nolock) on ca.primaryscoreid=sc.id
	left join
	dbo.score csc with (nolock) on ca.CoBuyerscoreid=csc.id
	inner join
	dbo.dataviewmodelscore dv with (nolock) on a.dataviewmodelscoreid=dv.id
	left join
	dbo.dataviewmodelscore cdv with (nolock) on coa.dataviewmodelscoreid=cdv.id
	inner join
	dbo.customer cu with (nolock)  on a.customerid=cu.id
	left join
	dbo.customer cuc with (nolock) on coa.customerid=cuc.id
	inner join
	dbo.address ad with (nolock) on a.addressid=ad.id
	left join
	dbo.address cad with (nolock) on coa.addressid=cad.id
	inner join
	dbo.streettype st with (nolock) on ad.streettypeid=st.id
	left join
	dbo.streettype cst with (nolock) on cad.streettypeid=cst.id
	inner join
	dbo.state sta with (nolock) on ad.stateid=sta.id
	left join
	dbo.state csta with (nolock) on cad.stateid=csta.id
	inner join
	dbo.PayingCapacity pc with (nolock) on a.payingcapacityid=pc.id
	left join
	dbo.PayingCapacity cpc with (nolock) on coa.payingcapacityid=cpc.id
	inner join
	dbo.Employment em with (nolock) on a.employmentid=em.id
	left join
	dbo.Employment cem with (nolock) on coa.employmentid=cem.id
WHERE 
	ca.id=@creditID AND ca.CreditAppStatusID>7




END
GO
/****** Object:  StoredProcedure [dbo].[PointPredictiveGetLastScore]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sam
-- Create date: 08/21/2020
-- Description:	Get current Point Predictive Score
--
-- EXEC PointPredictiveGetLastScore 236715; --282342; -- 236720;  --236627;  -- 236720  -- 248804
-- EXEC PointPredictiveGetLastScore 236720;
-- collect credit with status = "PendingPaycall"
-- select top 100 * from creditscoreapplication where CreditAppStatusID IN ( 8, 9) order by id desc;
-- =============================================
CREATE PROCEDURE [dbo].[PointPredictiveGetLastScore]  
	-- Add the parameters for the stored procedure here
	@creditID int
AS

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT TOP 1
		pp.[Id], 
		[CreditId], 
		[Response], 
		DATEDIFF(day,  [CreatedDate], GETDATE()) AS DaysSinceLastCall,
		pps.UWStatus AS [Status],
		pps.Id AS [UWStatusId],
		gss.GradeValue,
		gss.GradeLabel
	FROM [dbo].[PP_Return] pp 
	LEFT JOIN
		dbo.creditscoreapplication ca ON pp.CreditId = ca.id
	LEFT JOIN
		[dbo].[Score] s ON ca.PrimaryScoreID = s.Id
	LEFT JOIN
		[dbo].[GradeScoreSegment] gss on s.GradeScoreSegmentID = gss.id
	LEFT JOIN
		[dbo].[Grade_FraudMatrix] gfm ON gss.GradeValue = gfm.GradeValue
	LEFT JOIN
		[dbo].[PP_UWStatus] pps ON gfm.UWStatusID = pps.Id
	WHERE 
		pp.CreditId = @creditID 
		AND ca.CreditAppStatusID > 7 
		AND pp.FraudScore BETWEEN gfm.FraudScoreLow AND gfm.FraudScoreHigh
	ORDER BY
		pp.Id DESC;

END
GO
/****** Object:  StoredProcedure [dbo].[PointPredictiveGetUserID]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sam Aloni
-- Create date: 07/23/2020
-- Description:	Get user ID from user name
--  EXEC PointPredictiveGetUserID 'Sam';
--  EXEC PointPredictiveGetUserID 'USAUTOSALES\Amanda Barnett';
-- =============================================
CREATE PROCEDURE [dbo].[PointPredictiveGetUserID]
	-- Add the parameters for the stored procedure here
	@UserName varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @UserID int;

	SELECT TOP 1 @UserID = [Id] FROM [dbo].[User] WHERE [SPUser] = @UserName;

	IF @UserID IS NULL
		BEGIN
			SELECT TOP 1 @UserID = [Id] FROM [dbo].[User] WHERE [SPUser] = 'it@usauto-sales.com';
		END

    -- Insert statements for procedure here
	select @UserID;
END
GO
/****** Object:  StoredProcedure [dbo].[PointPredictiveSaveReqResp]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==================================================================
-- Author:		Sam Aloni
-- Create date: 08/21/2020
-- Description:	Save Point Predictive Request and response data
--
/*
	EXEC PointPredictiveSaveReqResp 206354,'reqString', 'respString', 
		'ssnString', 1,'FraudEpdReportLink','DealerRiskReportLink','456',
		'1','22','333','Clear req','*********',1,'Comment';
*/
-- ==================================================================
CREATE PROCEDURE [dbo].[PointPredictiveSaveReqResp] 
	@creditID int,
	@reqString varchar(MAX),
	@respString varchar(MAX),
	@ssnString varchar(MAX),
	@completed bit,
	@FraudEpdReportLink varchar(MAX),
	@DealerRiskReportLink varchar(MAX),
	@FraudScore varchar(25),
	@FraudReasonCode1 [varchar](10),
	@FraudReasonCode2 [varchar](10),
	@FraudReasonCode3 [varchar](10),
	@CleanReq varchar(MAX),
	@ErrorMsg varchar(MAX),
	@CreatedBy int ,
	@Comment varchar(MAX)



AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	-- collect Status information and load into PP_return table
	DECLARE @GradeValue int, @StatusID int, @fraudScoreInt int;

	--Added by TRM
	Declare @ScoreId int
	Declare @ScoreIDJoint int
	Declare @DPReduction float
	Declare @DPPct float
	Declare @DPJoint float
	Declare @AdjDPPct float
	Declare @AdjDPPctJoint float
	Declare @Previous int = 0

	SET @fraudScoreInt = CAST(@FraudScore AS int);

	SELECT TOP 1
		@GradeValue = gss.GradeValue,
		@StatusID = gfm.UWStatusID
	FROM 
		dbo.creditscoreapplication ca
	LEFT JOIN
		[dbo].[Score] s ON ca.PrimaryScoreID = s.Id
	LEFT JOIN
		[dbo].[GradeScoreSegment] gss on s.GradeScoreSegmentID = gss.id
	LEFT JOIN
		[dbo].[Grade_FraudMatrix] gfm ON gss.GradeValue = gfm.GradeValue
	WHERE 
		ca.Id = @creditID 
		AND ca.CreditAppStatusID > 7 
		AND @fraudScoreInt BETWEEN gfm.FraudScoreLow AND gfm.FraudScoreHigh

	-- SELECT @GradeValue , @StatusID

	INSERT INTO [dbo].[PP_Return]
           ([CreditId]
           ,[Completed]
           ,[CreatedDate]
           ,[Request]
           ,[Response]
           ,[SsnRespone]
           ,[Errors]
		   ,[FraudEpdReportLink]
		   ,[DealerRiskReportLink]
		   ,[FraudScore]
		   ,[FraudReasonCode1]
		   ,[FraudReasonCode2]
		   ,[FraudReasonCode3]
		   ,[CleanRequest]
		   ,[CreatedBy]
		   ,[GradeValue]
		   ,[StatusID]
		   )
		   OUTPUT Inserted.Id
		 VALUES
			(@creditID
			,@completed
			,GETDATE()
			,@reqString
			,@respString
			,@ssnString
			,@ErrorMsg
			,@FraudEpdReportLink
			,@DealerRiskReportLink
			,@FraudScore
			,@FraudReasonCode1
			,@FraudReasonCode2
			,@FraudReasonCode3
			,@CleanReq
			,@CreatedBy
			,@GradeValue
			,@StatusID
			)

	select @ScoreID=s.id,@DPReduction=m.DPReduction,@DPPct=s.MinDownPaymentPercent,@ScoreIdJoint=js.id,@DPJoint=js.MinDownPaymentPercent From 
	pp_return pp with (nolock)
	inner join CreditScoreApplication ca with (nolock) on pp.CreditId=ca.id
	inner join Score s with (nolock) on ca.PrimaryScoreID=s.id
	left join Score js on ca.JointScoreID=js.id
	inner join [Grade_FraudMatrix] m with (nolock) on pp.GradeValue=m.GradeValue and pp.StatusID=m.UWStatusID
	where ca.id=@creditID

	if @DPReduction>0
		begin
	
		insert into TLog values (0,getdate(),'StartProc')


		insert into TLog values (@creditID,getdate(),'Credit ID'+convert(varchar(10),@creditID))

			if @DPJoint>0
				begin
				
					select  @Previous=ID from dpchanges_hist where scoreid=@ScoreId

					insert into TLog values (@creditID,getdate(),'Previous ID='+convert(varchar(10),@Previous))
				
					if @Previous=0
						begin
						
							insert into TLog values (@creditID,getdate(),'In Joint')
							set @AdjDPPct=@DPPct-@DPReduction
							set @AdjDPPctJoint=@DPJoint-@DPReduction
				
							update score
							set MinDownPaymentPercent=@AdjDPPct,DateModified=GETDATE()
							where id=@ScoreId

							update score
							set MinDownPaymentPercent=@AdjDPPctJoint,DateModified=GETDATE()
							where id=@ScoreIDJoint

							insert into [dbo].[DPChanges_Hist] (CreditScoreID,ScoreID,JointScoreID,PrevDownPaymentPct,AdjDownPaymentPct,PrevDownPaymentPctJoint,AdjDownPaymentPctJoint,modifieddate)
							values (@creditID,@ScoreId,@ScoreIDJoint,@DPPct,@AdjDPPct,@DPJoint,@AdjDPPctJoint,getdate())

						
							insert into comment (Text,DateCreated,CreatedBy,CreditScoreApplicationID,DateModified,ModifiedBy)
							select  'DP % reduced to :'+convert(varchar(5),(@AdjDPPct*100))+'%',getdate(),1,@creditID,getdate(),1 

						END
			
				end
			else
				begin
					select  @Previous=ID from dpchanges_hist where scoreid=@ScoreId
				
					if @Previous=0
						begin
						   insert into TLog values (@creditID,getdate(),'In Single')
							set @AdjDPPct=@DPPct-@DPReduction 
				
							update score
							set MinDownPaymentPercent=@AdjDPPct,DateModified=GETDATE()
							where id=@ScoreId and id not in (select distinct scoreid from dpchanges_hist)

							insert into [dbo].[DPChanges_Hist] (CreditScoreID,ScoreID,PrevDownPaymentPct,AdjDownPaymentPct,modifieddate)
							values (@creditID,@ScoreId,@DPPct,@AdjDPPct,getdate()) 

							insert into comment (Text,DateCreated,CreatedBy,CreditScoreApplicationID,DateModified,ModifiedBy)
							select  'DP % reduced to :'+convert(varchar(5),(@AdjDPPct*100))+'%',getdate(),1,@creditID,getdate(),1 
						end 
				
				end
		end

END

GO
/****** Object:  StoredProcedure [dbo].[sp_getCreditScoreData]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_getCreditScoreData]
	 @PrimaryAppID  int
	,@CoBuyerAppID int = null
AS
BEGIN

	SET NOCOUNT ON;
	
	DECLARE @IsJointScore bit

	SELECT @IsJointScore = (CASE WHEN @CoBuyerAppID  IS NULL THEN 0 ELSE 1 END)
--------  Net Income and Income
	DECLARE @AvailableDownPayment decimal
	DECLARE @NetIncome decimal
	DECLARE @PrimaryIncome decimal
	
	SELECT 
	      @AvailableDownPayment = [AvailableDownPayment]
		 ,@NetIncome = [dbo].[fn_getNetIncome](p.PeriodPaycheck,p.SalaryTypeID, default)
		 ,@PrimaryIncome = [dbo].[fn_getNetIncome](p.PeriodPaycheck,p.SalaryTypeID,p.OtherIncome)	
	FROM dbo.Application as a
		JOIN dbo.PayingCapacity as p
		ON a.PayingCapacityID = p.Id
	WHERE 1=1
		AND  a.Id = @PrimaryAppID 

	DECLARE @CoIncome decimal
	IF  @IsJointScore = 1
	Begin
		SELECT 
			 @CoIncome = [dbo].[fn_getNetIncome](p.PeriodPaycheck,p.SalaryTypeID, default)	
		FROM dbo.Application as a
			JOIN dbo.PayingCapacity as p
			ON a.PayingCapacityID = p.Id
		WHERE 1=1
			AND  a.Id = @CoBuyerAppID
	End

	DECLARE @Income decimal
	SELECT @Income =  @PrimaryIncome + isnull(@CoIncome  * 0.25,0)

-------------- Score 	
	DECLARE @Score int, @BeaconScore varchar(50);
	SELECT 
		@Score = ModelScore 
		,@BeaconScore = BeaconScore
	FROM dbo.Application as a
		JOIN [dbo].[DataViewModelScore] as dv
		ON a.DataViewModelScoreID = dv.Id
	WHERE 1=1
		AND  a.Id = @PrimaryAppID 

    DECLARE @BeaconScoreNum int;
	IF @BeaconScore = ''
	BEGIN
		SET @BeaconScoreNum = 0;
	END
	ELSE
	BEGIN
		SET @BeaconScoreNum = CAST(@BeaconScore AS INT);
	END

	DECLARE @CoScore int
	IF  @IsJointScore = 1
	BEGIN
		SELECT 
			@CoScore = ModelScore 
		FROM dbo.Application as a
			JOIN [dbo].[DataViewModelScore] as dv
			ON a.DataViewModelScoreID = dv.Id
		WHERE 1=1
			AND  a.Id = @CoBuyerAppID;

		-- calculate joint beacon score
		SELECT @BeaconScoreNum = [dbo].[fn_getBeaconForJoint](@PrimaryAppID, @CoBuyerAppID);
	END
	
	DECLARE @ScoreResult int
	SELECT @ScoreResult = [dbo].[fn_getScore](@Score,@CoScore)

	-- Calculate the grading
	DECLARE @GradeScoreSegmentID  int;
	SELECT
		@GradeScoreSegmentID = [Id]
	FROM [dbo].[GradeScoreSegment]
	WHERE 1=1
		AND @ScoreResult BETWEEN [ScoreLow] AND [ScoreHigh];

	--------------------------------------------------------------
    -- New FICO Grading Layering
	--------------------------------------------------------------
	DECLARE @NewFicoLayeringGrade int;
	SELECT @NewFicoLayeringGrade = [dbo].[fn_getFicoLayeringGrade](@BeaconScoreNum, @GradeScoreSegmentID);

	DECLARE @SocreTypeID int;
	IF @IsJointScore = 0
	BEGIN
		SET @SocreTypeID = 1;		-- Applicant score
	END
    ELSE
	BEGIN
		SET @SocreTypeID = 3;		-- Joint score
	END

	-- Grading History audit
	INSERT INTO [dbo].[ScoreHistory]
		([ScoreTypeID], [Score], [ApplicationID], [DateModified], [GradeScoreSegmentID], [LastGradeScoreSegmentID], [BeaconScore])
		VALUES
		(@SocreTypeID , @ScoreResult, @PrimaryAppID, getdate(), @NewFicoLayeringGrade, @GradeScoreSegmentID, CAST(@BeaconScoreNum AS VARCHAR(50)));

-------------- 	@MaxMonthlyPayment and PTI
	DECLARE @PTI Float;
	SELECT DISTINCT TOP 1
		@PTI = [PTI]
	FROM [dbo].[GradeScoreSegment]
	WHERE 1=1
		AND id = @NewFicoLayeringGrade;

		-- For FICO Grade Layering need to use the GradeLabel
		--AND @ScoreResult BETWEEN [ScoreLow] AND [ScoreHigh]


	DECLARE @MaxMonthlyPayment decimal(10,2)
	-- [Sam 12-19-2019]
	-- The Max Monthly Payment should not be calculated from score. It should be calculated from the PTI above
	-- SELECT @MaxMonthlyPayment= [dbo].[fn_getMaxMonthlyPayment](@Income, @ScoreResult, @IsJointScore)
	--
	-- using the new Fico. We should used the PTI which may changed by the FICO layring. 

	SELECT @MaxMonthlyPayment= [dbo].[fn_getMaxMonthlyPayment](@Income, @PTI, @IsJointScore)

	DECLARE @PriorPurchaseOptionID int
 	SELECT 
		@PriorPurchaseOptionID =isnull(g.PriorPurchaseOptionID,0)
	FROM dbo.Application as a
		JOIN dbo.GeneralInfo as g
		ON a.Id =g.Id
	WHERE 1=1
		AND a.Id =@PrimaryAppID
	
	DECLARE @IRate float 
	-- The function needed to be changed for the FICO layering
	-- SELECT @IRate = [dbo].[fn_getIrate](@ScoreResult,@PriorPurchaseOptionID)
	SELECT @IRate = [dbo].[fn_getFicoIrate](@NewFicoLayeringGrade,@PriorPurchaseOptionID)

	DECLARE @MaxFinance DECIMAL 
	
	IF @IRate = 0
	BEGIN
		SET @MaxFinance = 0;
	END
	ELSE
	BEGIN
		SELECT @MaxFinance = [dbo].[fn_getMaxFinance](@MaxMonthlyPayment,@IRate, default)
	END

	DECLARE @MinDownPaymentPercent float  --- Note !!! default so no MaxPurchasePriceByMonthlyPayment = Sales Price
	--SELECT @MinDownPaymentPercent = [dbo].[fn_getDownPaymentPercent](@GradeScoreSegmentID, default)
	SELECT @MinDownPaymentPercent = [dbo].[fn_getDownPaymentPercent](@NewFicoLayeringGrade, default)
	--SELECT @MinDownPaymentPercent = [dbo].[fn_getDownPaymentPercent_score](@PrimaryAppID)
	DECLARE @Fees decimal
	SELECT @Fees = [dbo].[fn_getFees](@PrimaryAppID)

	DECLARE @MaxPurchasePrice decimal
	SELECT @MaxPurchasePrice = [dbo].[fn_getMaxPurchasePrice](@MaxFinance,@MinDownPaymentPercent,@Fees,default)
	
	DECLARE @MaxPurchasePriceZeroDownPayment decimal
	SELECT @MaxPurchasePriceZeroDownPayment = [dbo].[fn_getMaxPurchasePrice](@MaxFinance,0,@Fees,default)

	DECLARE @PaymentTypeID int
	SELECT @PaymentTypeID = [dbo].[fn_getPaymentTypeID](@PrimaryAppID)

	DECLARE @MaxPickPayment decimal
	SELECT @MaxPickPayment = dbo.fn_getPicks(@MaxMonthlyPayment, @PaymentTypeID)

	SELECT
		@IsJointScore as [IsJointScore]
		,@ScoreResult as [Score]
		,@PrimaryAppID as [ApplicationID]
		,@AvailableDownPayment as [AvailableDownPayment]
		,@NetIncome as [NetIncome]
		,@Income as [Income]
		,@PTI as [PTI]
		,@MaxMonthlyPayment as [MaxMonthlyPayment]
		,@PriorPurchaseOptionID as [PriorPurchaseOptionID]
		,@IRate as [IRate]
		,@MaxFinance as [MaxFinance]
		-- ,@GradeScoreSegmentID as [GradeScoreSegmentID]
		,@NewFicoLayeringGrade as [GradeScoreSegmentID]
		,@MinDownPaymentPercent as [MinDownPaymentPercent]
		,@Fees as [Fees]
		,@MaxPurchasePrice as [MaxPurchasePrice]
		,@MaxPurchasePriceZeroDownPayment as [MaxPurchasePriceZeroDownPayment]
		,@MaxPickPayment as [MaxPickPayment];

END





GO
/****** Object:  StoredProcedure [dbo].[sp_getFilteredVehicles]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_getFilteredVehicles]
    @ScoreID int,
    @LotId int,
    @AdjustedCashUpFront decimal = null,
    @AdjustedPickPayment decimal = null,
    @AdjustedIRate Float = null,
    @TradeACV decimal = 0,
    @StateId int = null
AS
BEGIN

    SET NOCOUNT ON;

    DECLARE @ApplicationID int
    DECLARE @Score decimal
    DECLARE @MonthlyPayment decimal
    DECLARE @Income decimal
    DECLARE @CashUpFront decimal
    DECLARE @MinDownPaymentPercent float
    DECLARE @GradeScoreSegmentID int
    DECLARE @IRate Float
    DECLARE @PurchasePrice decimal
    DECLARE @Fees decimal
    DECLARE @PickPayment decimal

    SELECT
      @ApplicationID = ApplicationID
      ,@Score = [Score]
      ,@Income = [Income]
      ,@MonthlyPayment = (
        case when 
            AdjustedMonthlyPayment is null 
            or AdjustedMonthlyPayment = 0
            or AdjustedMonthlyPayment >= MaxMonthlyPayment 
            then MaxMonthlyPayment 
            else AdjustedMonthlyPayment 
        end )
      ,@CashUpFront = (
        case when 
            AdjustedDownPayment is null 
            or AdjustedDownPayment = 0
            or AdjustedDownPayment >= AvailableDownPayment
            then AvailableDownPayment
            else  AdjustedDownPayment
        end )
      ,@MinDownPaymentPercent = [MinDownPaymentPercent]
      ,@GradeScoreSegmentID = [GradeScoreSegmentID]
      ,@IRate = [IRate]
      ,@PurchasePrice = (
        case when 
            [AdjustedPurchasePrice] is null 
            or [AdjustedPurchasePrice] = 0
            or [AdjustedPurchasePrice] >= [MaxPurchasePrice] 
            then [MaxPurchasePrice]
            else [AdjustedPurchasePrice]
        end )
      ,@PickPayment = (
        case when 
            [AdjustedPickPayment] is  null 
            or [AdjustedPickPayment] = 0
            or [AdjustedPickPayment] >= [MaxPurchasePrice] 
            then [MaxPickPayment]
            else [AdjustedPickPayment]
        end )
      ,@Fees = [Fees]
    FROM dbo.Score
    WHERE 1=1
        AND Id = @ScoreID
    
    --SELECT
 --     @ApplicationID 
    --  ,@Score
 --     ,@Income 
 --     ,@MinDownPaymentPercent 
 --     ,@GradeScoreSegmentID
 --     ,@IRate 
 --     ,@MonthlyPayment 
    --  ,@CashUpFront 
 --     ,@PurchasePrice 
    --  ,@PickPayment 
 --     ,@Fees 

    --Set Filter params from Calculator
    --more then mindown payment
    select  @CashUpFront = (    
    case when @AdjustedCashUpFront is not null 
        --or @AdjustedCashUpFront >= @CashUpFront -- TODO: check
        then @AdjustedCashUpFront
        else @CashUpFront
    end )

    --less then original
    select  @PickPayment = (    
    case when @AdjustedPickPayment is not null 
        --or @AdjustedPickPayment <= @PickPayment -- TODO: check
        then @AdjustedPickPayment
        else @PickPayment
    end )

    --less then original
    select  @IRate = (    
    case when @AdjustedIRate is not null 
        --or @AdjustedIRate < @IRate -- TODO: check
        then @AdjustedIRate
        else @IRate
    end )


    DECLARE @PaymentTypeID int
    SELECT @PaymentTypeID = [dbo].[fn_getPaymentTypeID](@ApplicationID)
    
    
    DECLARE @CustomerDownPayment decimal
    select @CustomerDownPayment = [dbo].[fn_getDownPayment](@CashUpFront,@PickPayment,@TradeACV)

    --select 
    --@CashUpFront
    --,@PickPayment
    --,@IRate
    --,@PaymentTypeID 
    --,@CustomerDownPayment
    --print @PurchasePrice
    --print @CustomerDownPayment

    -- select year for cpecific lot and grade
    declare @FromYear varchar(4)
    select @FromYear = [dbo].[fn_getFromModelYear](@StateId, @GradeScoreSegmentID)

    SELECT 
        *
    FROM (
        SELECT
            *
            ,[dbo].[fn_getMinDownPaymentBySellingPrice](@GradeScoreSegmentID,T1.SellingPrice) as [MinDownPayment]           
            ,[dbo].[fn_getPayment](T1.SellingPrice,@Fees,T1.Warranty0,@CustomerDownPayment,@IRate,[MaxTermNoVSC],@PaymentTypeID) as [VSCNO]
            ,[dbo].[fn_getPayment](T1.SellingPrice,@Fees,T1.Warranty24,@CustomerDownPayment,@IRate,[MaxTermVSC],@PaymentTypeID) as [VSC2]
            ,[dbo].[fn_getPayment](T1.SellingPrice,@Fees,T1.Warranty36,@CustomerDownPayment,@IRate,[MaxTermVSC],@PaymentTypeID) as [VSC3]
        FROM (
         SELECT 
            vi.Id
            ,vi.StockNo
            ,vi.CarYear
            ,vi.CarMake
            ,vi.CarModel
            ,vi.Mileage
            ,vi.DaysInInven
            ,l.Id as [LotID]
            ,l.LotName
            ,vi.SalesPrice as [SellingPrice]
            ,[dbo].[fn_getMaxTerm] (oct.CarTypeSegmentID, vi.Mileage,1) as [MaxTermNoVsc]
            ,[dbo].[fn_getMaxTerm] (oct.CarTypeSegmentID, vi.Mileage,0) as [MaxTermVSC]
            ,[dbo].[fn_getWarrantyPrice](0) as [Warranty0]
            ,[dbo].[fn_getWarrantyPrice](24) as [Warranty24]
            ,[dbo].[fn_getWarrantyPrice](36) as [Warranty36]
        FROM dbo.VehicleInventory as vi
            JOIN dbo.OriginalCarType as oct
                on vi.OriginalCarTypeID = oct.Id
            JOIN dbo.CarTypeSegment as cts
                on oct.CarTypeSegmentID =  cts.Id
            JOIN dbo.LOT as l
                on vi.LotID = l.Id
        WHERE vi.IsSellingEnabled =1 or  vi.IsSellingEnabled is NULL
        ) AS T1
    ) AS T 
    WHERE 1=1
        AND (T.LotID = @LotID  or @LotID is null or @LotID = 99) -- 99 instead of 21 in prod
        AND T.SellingPrice <= @PurchasePrice
        AND [MinDownPayment] < @CustomerDownPayment
        AND T.CarYear <= @FromYear
END
GO
/****** Object:  StoredProcedure [dbo].[sp_getVehicleDetails]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_getVehicleDetails]
	@ScoreID int,
	@VehicleId int,
	@AdjustedCashUpFront decimal = null,
	@AdjustedPickPayment decimal = null,
	@AdjustedIRate Float = null,
	@LotID int = null,
	@WarrantyID int = NULL,
	@TradeACV decimal = 0,
	@TradePayoff decimal = 0
AS
BEGIN

	SET NOCOUNT ON;
	DECLARE @ApplicationID int
	DECLARE @Score decimal
    DECLARE @MonthlyPayment decimal
    DECLARE @Income decimal
	DECLARE @CashUpFront decimal
	DECLARE @MinDownPaymentPercent float
	DECLARE @GradeScoreSegmentID int
	DECLARE @IRate Float
	DECLARE @PurchasePrice decimal
	DECLARE @Fees decimal
	DECLARE @PickPayment decimal

	SELECT
      @ApplicationID = ApplicationID
	  ,@Score = [Score]
      ,@Income = [Income]
      ,@MonthlyPayment = (
	    case when 
			AdjustedMonthlyPayment is null 
			or AdjustedMonthlyPayment = 0
			or AdjustedMonthlyPayment >= MaxMonthlyPayment 
			then MaxMonthlyPayment 
			else AdjustedMonthlyPayment 
		end )
	  ,@CashUpFront = (
	    case when 
			AdjustedDownPayment is null 
			or AdjustedDownPayment = 0
			or AdjustedDownPayment >= AvailableDownPayment
			then AvailableDownPayment
			else  AdjustedDownPayment
		end )
      ,@MinDownPaymentPercent = [MinDownPaymentPercent]
      ,@GradeScoreSegmentID = [GradeScoreSegmentID]
      ,@IRate = [IRate]
      ,@PurchasePrice = (
	    case when 
			[AdjustedPurchasePrice] is null 
			or [AdjustedPurchasePrice] = 0
			or [AdjustedPurchasePrice] >= [MaxPurchasePrice] 
			then [MaxPurchasePrice]
			else [AdjustedPurchasePrice]
		end )
	  ,@PickPayment = (
	    case when 
			[AdjustedPickPayment] is null 
			or [AdjustedPickPayment] = 0
			or [AdjustedPickPayment] >= [MaxPurchasePrice] 
			then [MaxPickPayment]
			else [AdjustedPickPayment]
		end )
      ,@Fees = [Fees]
	FROM dbo.Score
	WHERE 1=1
		AND Id = @ScoreID

	--Set Filter params from Calculator
	--more then mindown payment
	select 	@CashUpFront = (    
	case when @AdjustedCashUpFront is not null 
		--or @AdjustedCashUpFront >= @CashUpFront -- TODO: check
		then @AdjustedCashUpFront
		else @CashUpFront
	end )

	--less then original
	select 	@PickPayment = (    
	case when @AdjustedPickPayment is not null 
		--or @AdjustedPickPayment <= @PickPayment -- TODO: check
		then @AdjustedPickPayment
		else @PickPayment
	end )

	--less then original
	select 	@IRate = (    
	case when @AdjustedIRate is not null 
		--or @AdjustedIRate < @IRate -- TODO: check
		then @AdjustedIRate
		else @IRate
	end )


	DECLARE @PaymentTypeID int
	SELECT @PaymentTypeID = [dbo].[fn_getPaymentTypeID](@ApplicationID)
	
	DECLARE @CustomerDownPayment decimal
	select @CustomerDownPayment = [dbo].[fn_getDownPayment](@CashUpFront,@PickPayment,@TradeACV)
	
	DECLARE @WarrantyPrice decimal
	DECLARE @IsNoVSC int
	SELECT 
		@WarrantyPrice = StandardPrice
		,@IsNoVSC = IsNoVSC
	FROM dbo.Warranty
	WHERE 1=1
		AND Id = @WarrantyID

	SELECT 
		@MonthlyPayment as [MonthlyPayment]
		,@CashUpFront as [DownPayment]
		,@IRate as [InterestRate]
		,@PaymentTypeID as [PaymentTypeID]
		,@PickPayment as [PickPayment] 		
		,@WarrantyID as [WarrantyID]
		,@WarrantyPrice as [WarrantyPrice]
		,@TradeACV as [TradeACV]
		,@TradePayoff as [TradePayoff]
        ,[dbo].[fn_getPayment](T.SellingPrice,@Fees,@WarrantyPrice,@CustomerDownPayment,@IRate,[MaxTerm],@PaymentTypeID) as [Payment]	
		,*
	FROM (
		SELECT
			*
			,(CASE WHEN @IsNoVSC = 1 
				THEN [MaxTermNoVsc]
				ELSE [MaxTermVSC]
		     END)  as [MaxTerm]
			,[dbo].[fn_getMinDownPaymentBySellingPrice](@GradeScoreSegmentID,T1.SellingPrice) as [MinDownPayment]		
			,[dbo].[fn_getPayment](T1.SellingPrice,@Fees,T1.Warranty0,@CustomerDownPayment,@IRate,[MaxTermNoVsc],@PaymentTypeID) as [VSCNO]
			,[dbo].[fn_getPayment](T1.SellingPrice,@Fees,T1.Warranty24,@CustomerDownPayment,@IRate,[MaxTermVSC],@PaymentTypeID) as [VSC2]
			,[dbo].[fn_getPayment](T1.SellingPrice,@Fees,T1.Warranty36,@CustomerDownPayment,@IRate,[MaxTermVSC],@PaymentTypeID) as [VSC3]
		FROM (
		 SELECT	
			vi.Id
			,vi.StockNo
			,vi.CarYear
			,vi.CarMake
			,vi.CarModel
			,vi.Mileage
			,vi.SalesPrice as [SellingPrice]
			,vi.CarColor
			,vi.EngineCycles
			,vi.FuelType
			,vi.Transmission
			,vi.Vin
			,vi.LotID
			,vi.DaysInInven
			,cts.CarType
			,l.LotName
			,[dbo].[fn_getMaxTerm] (oct.CarTypeSegmentID, vi.Mileage,1) as [MaxTermNoVsc]
			,[dbo].[fn_getMaxTerm] (oct.CarTypeSegmentID, vi.Mileage,0) as [MaxTermVSC]
			,[dbo].[fn_getWarrantyPrice](0) as [Warranty0]
			,[dbo].[fn_getWarrantyPrice](24) as [Warranty24]
			,[dbo].[fn_getWarrantyPrice](36) as [Warranty36]
		FROM dbo.VehicleInventory as vi
			JOIN dbo.OriginalCarType as oct
				on vi.OriginalCarTypeID = oct.Id
			JOIN dbo.CarTypeSegment as cts
				on oct.CarTypeSegmentID =  cts.Id
			JOIN dbo.LOT as l
				on vi.LotID = l.Id
		) AS T1
	) AS T 
	WHERE 1=1
		AND T.Id = @VehicleID OR (
			@VehicleID is NULL
			AND (T.LotID = @LotID or @LotID is null or @LotID =99)
			AND  T.SellingPrice <= @PurchasePrice
			AND  [MinDownPayment] < @CustomerDownPayment
		)
END





GO
/****** Object:  StoredProcedure [dbo].[TrustScienceGetApplicationDetail]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =========================================================================================================================
-- Author:		Travis / Sam Aloni
-- Create date: 10/13/2020
-- Description:	Collect the batch of applications to process since the last time we processed the batch (Once an hour) 
-- EXEC TrustScienceGetApplicationDetail 294084 ;
-- =========================================================================================================================
CREATE PROCEDURE [dbo].[TrustScienceGetApplicationDetail]
	@CreditScoreApplicationID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	SELECT * FROM
	(
	SELECT DISTINCT
		 st.StateAbbreviation as jurisdictionState,
		 upper(isNull(c.firstName,'')) as firstName,
		 upper(isNull(c.middleName,'')) as middleName,
		 upper(isNull(c.lastName,'')) as lastName,
		 upper(ad.HouseNumber + ' '+ rtrim(ltrim(ad.StreetName)) +' '+rtrim(ltrim(stt.StreetTypeName))) as streetAddress1,
		 upper(isNull(ad.city,'')) as city1,
		 isNull(st.StateAbbreviation ,'') as state1,
		 substring(ad.postalcode,1,5) as postalCode1,
		 country1='USA',
		 ad.totalmonths as monthsAtResidence1,
		 ht.HousingTypeName as residenceStatus1,
		 'TRUE' as isCurrentResidence,
		 upper(rtrim(ltrim(isNull(isNull(ad.PrevStreetName,ad.PrevAddressLine),'')))) as streetAddress2,
		 upper(isNull(ad.prevcity,'')) as city2,
		 isNull(st1.StateAbbreviation ,'') as state2,
		 isNull(substring(ad.prevpostalcode,1,5),'') as postalCode2,
		 country2='USA',
		 ht.HousingTypeName as residenceStatus2,
		isNull(replace(replace(replace(replace(c.mobilephone,'-',''),'(',''),')',''),' ',''),'') as mobilePhone,
		isNull(replace(replace(replace(replace(c.homephone,'-',''),'(',''),')',''),' ',''),'') as homePhone, 
		isNull(replace(replace(replace(replace(e.workphone,'-',''),'(',''),')',''),' ',''),'') as workPhone,
		upper(isNull(c.emailaddress,'')) as email,
		case when c.dateofbirth is null then ''
		else convert(varchar(10),c.dateofbirth,121)
		end as birthday,
		ca.id as applicationID,
		c.ssn as SSN ,
		isNull(c.DriverLicenseNumber,'') as driverLicenseNumber,
		isNull(st2.StateAbbreviation,'') as issueState,
		isNull(case when isNull(cast(c.DriverlicenseExpirationDate as varchar(10)),'')='1900-01-01' then '' else cast(c.DriverlicenseExpirationDate as varchar(10)) end,'')  as  driverlicenseExpirationDate,
		upper(isNull(e.name,'')) as employerName,
		isNull(replace(replace(replace(e.workphone,'-',''),'(',''),')',''),'') as employerPhone,
		upper(isNull(e.Position,'')) as jobTitle,
		'TRUE' as isCurrentlyEmployed,
		s.netincome as monthlyIncomeNet,
		e.totalmonths as employmentMonthCount,
		case pc.paymenttypeid when '1' then '52' when '2' then '26' when '3' then '24' else '12' end as paymentsPerYear,
		convert(varchar(10),ca.datecreated,121) as dateOriginated,
		'60' as term,
		c.id as clientCustomerId,
		ca.id as clientLoanReferenceId,
		a.id as clientApplicationId,
		s.maxpurchaseprice as maxPurchasePrice, 
		case 
			when isNull(s.AdjustedMonthlyPayment,0) > 0 then s.AdjustedMonthlyPayment 
			else s.MaxMonthlyPayment 
		end as maxMonthlyPayment,
		s.MinDownPaymentPercent AS minDownPaymentPercent,
		s.maxpurchaseprice as principalAmount,
		s.irate as annualInterestRate,
		upper(lot.LotName) as lotName,
		lot.streetAddress,
		lot.city,
		lot.postalCode,
		lot.[state],
		dv.outputxml,
		ca.datemodified AS dateModified
 
	FROM 
		 CreditScoreApplication ca with (nolock) 
		 inner join [Application] a with (nolock)  on ca.primarybuyerapplicationid=a.id
		 inner join Score s with (nolock)  on ca.primaryscoreid=s.id
		 inner join Customer c with (nolock)  on a.customerid=c.id
		 inner join [Address] ad with (nolock)  on a.addressid=ad.id
		 inner join employment e with (nolock)  on a.employmentid=e.id
		 inner join payingcapacity pc with (nolock)  on a.payingcapacityid=pc.id
		 inner join [state] st with (nolock)  on ad.stateid=st.id
		 left join  [state] st1 with (nolock)  on ad.prevstateid=st1.id
		 left join [state] st2 with (nolock)  on c.DriverLicenseStateID=st2.id
		 inner join streettype stt with (nolock)  on ad.streettypeid=stt.id
		 inner join housingtype ht with (nolock)  on pc.housingtypeid=ht.id
		 inner join lot lot with (nolock)  on ca.lotid=lot.id
		 inner join DataviewModelScore dv with (nolock)  on a.Dataviewmodelscoreid=dv.id
 
	 WHERE 
		ca.id = @CreditScoreApplicationID

 UNION ALL
  
  SELECT 
		 st.StateAbbreviation as jurisdictionState,
		 upper(isNull(c.firstName,'')) as firstName,
		 upper(isNull(c.middleName,'')) as middleName,
		 upper(isNull(c.lastName,'')) as lastName,
		 upper(ad.HouseNumber + ' '+ rtrim(ltrim(ad.StreetName)) +' '+rtrim(ltrim(stt.StreetTypeName))) as StreetAddress1,
		 upper(isNull(ad.city,'')) as city1,
		 isNull(st.StateAbbreviation ,'') as state1,
		 substring(ad.postalcode,1,5) as postalCode1,
		 country1='USA',
		 ad.totalmonths as monthsAtResidence1,
		 ht.HousingTypeName as residenceStatus1,
		 'TRUE' as isCurrentResidence,
		 upper(rtrim(ltrim(isNull(isNull(ad.PrevStreetName,ad.PrevAddressLine),'')))) as streetAddress2,
		 upper(isNull(ad.prevcity,'')) as city2,
		 isNull(st1.StateAbbreviation ,'') as state2,
		 isNull(substring(ad.prevpostalcode,1,5),'') as postalCode2,
		 country2='USA',
		 ht.HousingTypeName as residenceStatus2,
		isNull(replace(replace(replace(replace(c.mobilephone,'-',''),'(',''),')',''),' ',''),'') as mobilePhone,
		isNull(replace(replace(replace(replace(c.homephone,'-',''),'(',''),')',''),' ',''),'') as homePhone, 
		isNull(replace(replace(replace(replace(e.workphone,'-',''),'(',''),')',''),' ',''),'') as workPhone,
		upper(isNull(c.emailaddress,'')) as email,
		case when c.dateofbirth is null then ''
		else convert(varchar(10),c.dateofbirth,121)
		end as birthday,
		ca.id as applicationID,
		c.ssn as SSN ,
		isNull(c.DriverLicenseNumber,'') as driverLicenseNumber,
		isNull(st2.StateAbbreviation,'') as issueState,
		isNull(case when isNull(cast(c.DriverlicenseExpirationDate as varchar(10)),'')='1900-01-01' then '' else cast(c.DriverlicenseExpirationDate as varchar(10)) end,'')  as  driverlicenseExpirationDate,
		upper(isNull(e.name,'')) as employerName,
		isNull(replace(replace(replace(e.workphone,'-',''),'(',''),')',''),'') as employerPhone,
		upper(isNull(e.Position,'')) as jobTitle,
		'TRUE' as isCurrentlyEmployed,
		s.netincome as monthlyIncomeNet,
		e.totalmonths as employmentMonthCount,
		case pc.paymenttypeid when '1' then '52' when '2' then '26' when '3' then '24' else '12' end as paymentsPerYear,
		convert(varchar(10),ca.datecreated,121) as dateOriginated,
		'60' as term,
		c.id as clientCustomerId,
		ca.id as clientLoanReferenceId,
		a.id as clientApplicationId,
		s.maxpurchaseprice as maxPurchasePrice, 
		case 
			when isNull(s.AdjustedMonthlyPayment,0)>0 then s.AdjustedMonthlyPayment 
			else s.MaxMonthlyPayment 
		end as maxMonthlyPayment,
		s.MinDownPaymentPercent AS minDownPaymentPercent,
		s.maxpurchaseprice as principalAmount,
		s.irate as annualInterestRate,
		upper(lot.LotName) as lotName,
		lot.streetAddress,
		lot.city,
		lot.postalCode,
		lot.[state],
		dv.outputxml,
		ca.datemodified AS dateModified
 
	FROM
	 CreditScoreApplication ca with (nolock) 
	 inner join Application a with (nolock)  on ca.cobuyerapplicationid=a.id
	 inner join Score s with (nolock)  on ca.primaryscoreid=s.id
	 inner join Customer c with (nolock)  on a.customerid=c.id
	 inner join Address ad  with (nolock) on a.addressid=ad.id
	 inner join employment e with (nolock)  on a.employmentid=e.id
	 inner join payingcapacity pc with (nolock)  on a.payingcapacityid=pc.id
	 inner join [state] st with (nolock)  on ad.stateid=st.id
	 left join  [state] st1 with (nolock)  on ad.prevstateid=st1.id
	 left join [state] st2 with (nolock)  on c.DriverLicenseStateID=st2.id
	 inner join streettype stt with (nolock)  on ad.streettypeid=stt.id
	 inner join housingtype ht with (nolock)  on pc.housingtypeid=ht.id
	 inner join lot lot with (nolock)  on ca.lotid=lot.id
	 inner join DataviewModelScore dv with (nolock)  on a.Dataviewmodelscoreid=dv.id
 
	WHERE 
	 ca.id = @CreditScoreApplicationID
	 ) b;


END
GO
/****** Object:  StoredProcedure [dbo].[TrustScienceGetBatchApplicationsToProcess]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =========================================================================================================================
-- Author:		Sam Aloni
-- Create date: 11/04/2020
-- Description:	Collect the batch of applications to process since the last time we processed the batch (Once an hour) 
-- EXEC TrustScienceGetBatchApplicationsToProcess;
-- =========================================================================================================================
CREATE PROCEDURE [dbo].[TrustScienceGetBatchApplicationsToProcess]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	-- find the list date we processed
	DECLARE @ProcessLastDataTime datetime;

	SELECT TOP 1 @ProcessLastDataTime = [LastItemDateTime] FROM [dbo].[TrustScienceScoreProcessingResult] ORDER BY [LastItemDateTime] DESC;

	IF @ProcessLastDataTime IS NULL
	  BEGIN
		--SET @ProcessLastDataTime = DATEADD(HOUR, -1, GETDATE());
		SET  @ProcessLastDataTime =  CONVERT(date, getdate())
	  END

	--SET @ProcessLastDataTime = '2020-11-04';
	--SELECT @ProcessLastDataTime

	SELECT CreditScoreApplicationID, dateModified FROM
	(
	SELECT DISTINCT
		ca.id as CreditScoreApplicationID,
		ca.datemodified AS dateModified,
		c.Id AS customerID
 
	FROM 
		 CreditScoreApplication ca with (nolock) 
		 inner join [Application] a with (nolock)  on ca.primarybuyerapplicationid=a.id
		 inner join Score s with (nolock)  on ca.primaryscoreid=s.id
		 inner join Customer c with (nolock)  on a.customerid=c.id
		 inner join [Address] ad with (nolock)  on a.addressid=ad.id
		 inner join employment e with (nolock)  on a.employmentid=e.id
		 inner join payingcapacity pc with (nolock)  on a.payingcapacityid=pc.id
		 inner join [state] st with (nolock)  on ad.stateid=st.id
		 left join  [state] st1 with (nolock)  on ad.prevstateid=st1.id
		 left join [state] st2 with (nolock)  on c.DriverLicenseStateID=st2.id
		 inner join streettype stt with (nolock)  on ad.streettypeid=stt.id
		 inner join housingtype ht with (nolock)  on pc.housingtypeid=ht.id
		 inner join lot lot with (nolock)  on ca.lotid=lot.id
		 inner join DataviewModelScore dv with (nolock)  on a.Dataviewmodelscoreid=dv.id
 
 
	 WHERE 
	 ca.datemodified > @ProcessLastDataTime    -- BETWEEN '2019-10-02' AND '2019-10-03' AND
	 AND creditappstatusid >= 4 

 UNION ALL
  
  SELECT 
		ca.id as CreditScoreApplicationID,
		ca.datemodified AS DateModified,
		c.Id AS customerID
 
	FROM
	 CreditScoreApplication ca with (nolock) 
	 inner join Application a with (nolock)  on ca.cobuyerapplicationid=a.id
	 inner join Score s with (nolock)  on ca.primaryscoreid=s.id
	 inner join Customer c with (nolock)  on a.customerid=c.id
	 inner join Address ad  with (nolock) on a.addressid=ad.id
	 inner join employment e with (nolock)  on a.employmentid=e.id
	 inner join payingcapacity pc with (nolock)  on a.payingcapacityid=pc.id
	 inner join [state] st with (nolock)  on ad.stateid=st.id
	 left join  [state] st1 with (nolock)  on ad.prevstateid=st1.id
	 left join [state] st2 with (nolock)  on c.DriverLicenseStateID=st2.id
	 inner join streettype stt with (nolock)  on ad.streettypeid=stt.id
	 inner join housingtype ht with (nolock)  on pc.housingtypeid=ht.id
	 inner join lot lot with (nolock)  on ca.lotid=lot.id
	 inner join DataviewModelScore dv with (nolock)  on a.Dataviewmodelscoreid=dv.id
 
	WHERE 
	 ca.datemodified > @ProcessLastDataTime    -- BETWEEN '2019-10-02' AND '2019-10-03' AND
	 AND creditappstatusid >= 4 
	 ) b 
	 WHERE NOT EXISTS (
						 SELECT CustomerID -- ApplicationID , CreateDate
						  FROM [ScoringDBProd].[dbo].[TrustScienceScore]
						  WHERE 
							CreateDate > DATEADD(DAY, -30, GETDATE())
							AND b.customerID = CustomerID
					   )
	 ORDER BY dateModified ASC;

END
GO
/****** Object:  StoredProcedure [dbo].[TrustScienceGetBatchToProcess]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =========================================================================================================================
-- Author:		Travis / Sam Aloni
-- Create date: 10/13/2020
-- Description:	Collect the batch of applications to process since the last time we processed the batch (Once an hour) 
-- EXEC TrustScienceGetBatchToProcess;
-- =========================================================================================================================
create PROCEDURE [dbo].[TrustScienceGetBatchToProcess]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	-- find the list date we processed
	DECLARE @ProcessLastDataTime datetime;

	SELECT TOP 1 @ProcessLastDataTime = [LastItemDateTime] FROM [dbo].[TrustScienceScoreProcessingResult] ORDER BY [LastItemDateTime] DESC;

	IF @ProcessLastDataTime IS NULL
	  BEGIN
		--SET @ProcessLastDataTime = DATEADD(HOUR, -1, GETDATE());
		SET  @ProcessLastDataTime =  CONVERT(date, getdate())
	  END

	-- *SELECT @ProcessLastDataTime*

	--------------------------------------
	---     REMOVE FIXED DATE  
	--------------------------------------
	SET @ProcessLastDataTime = '2020-08-27';

	SELECT top 10 * FROM
	(
	SELECT DISTINCT
		 st.StateAbbreviation as jurisdictionState,
		 upper(isNull(c.firstName,'')) as firstName,
		 upper(isNull(c.middleName,'')) as middleName,
		 upper(isNull(c.lastName,'')) as lastName,
		 upper(ad.HouseNumber + ' '+ rtrim(ltrim(ad.StreetName)) +' '+rtrim(ltrim(stt.StreetTypeName))) as streetAddress1,
		 upper(isNull(ad.city,'')) as city1,
		 isNull(st.StateAbbreviation ,'') as state1,
		 substring(ad.postalcode,1,5) as postalCode1,
		 country1='USA',
		 ad.totalmonths as monthsAtResidence1,
		 ht.HousingTypeName as residenceStatus1,
		 'TRUE' as isCurrentResidence,
		 upper(rtrim(ltrim(isNull(isNull(ad.PrevStreetName,ad.PrevAddressLine),'')))) as streetAddress2,
		 upper(isNull(ad.prevcity,'')) as city2,
		 isNull(st1.StateAbbreviation ,'') as state2,
		 isNull(substring(ad.prevpostalcode,1,5),'') as postalCode2,
		 country2='USA',
		 ht.HousingTypeName as residenceStatus2,
		isNull(replace(replace(replace(replace(c.mobilephone,'-',''),'(',''),')',''),' ',''),'') as mobilePhone,
		isNull(replace(replace(replace(replace(c.homephone,'-',''),'(',''),')',''),' ',''),'') as homePhone, 
		isNull(replace(replace(replace(replace(e.workphone,'-',''),'(',''),')',''),' ',''),'') as workPhone,
		upper(isNull(c.emailaddress,'')) as email,
		case when c.dateofbirth is null then ''
		else convert(varchar(10),c.dateofbirth,121)
		end as birthday,
		ca.id as applicationID,
		c.ssn as SSN ,
		isNull(c.DriverLicenseNumber,'') as driverLicenseNumber,
		isNull(st2.StateAbbreviation,'') as issueState,
		isNull(case when isNull(cast(c.DriverlicenseExpirationDate as varchar(10)),'')='1900-01-01' then '' else cast(c.DriverlicenseExpirationDate as varchar(10)) end,'')  as  driverlicenseExpirationDate,
		upper(isNull(e.name,'')) as employerName,
		isNull(replace(replace(replace(e.workphone,'-',''),'(',''),')',''),'') as employerPhone,
		upper(isNull(e.Position,'')) as jobTitle,
		'TRUE' as isCurrentlyEmployed,
		s.netincome as monthlyIncomeNet,
		e.totalmonths as employmentMonthCount,
		case pc.paymenttypeid when '1' then '52' when '2' then '26' when '3' then '24' else '12' end as paymentsPerYear,
		convert(varchar(10),ca.datecreated,121) as dateOriginated,
		'60' as term,
		c.id as clientCustomerId,
		ca.id as clientLoanReferenceId,
		a.id as clientApplicationId,
		s.maxpurchaseprice as maxPurchasePrice, 
		case 
			when isNull(s.AdjustedMonthlyPayment,0) > 0 then s.AdjustedMonthlyPayment 
			else s.MaxMonthlyPayment 
		end as maxMonthlyPayment,
		s.MinDownPaymentPercent AS minDownPaymentPercent,
		s.maxpurchaseprice as principalAmount,
		s.irate as annualInterestRate,
		upper(lot.LotName) as lotName,
		lot.streetAddress,
		lot.city,
		lot.postalCode,
		lot.[state],
		dv.outputxml,
		ca.datemodified AS dateModified
 
	FROM 
		 CreditScoreApplication ca with (nolock) 
		 inner join [Application] a with (nolock)  on ca.primarybuyerapplicationid=a.id
		 inner join Score s with (nolock)  on ca.primaryscoreid=s.id
		 inner join Customer c with (nolock)  on a.customerid=c.id
		 inner join [Address] ad with (nolock)  on a.addressid=ad.id
		 inner join employment e with (nolock)  on a.employmentid=e.id
		 inner join payingcapacity pc with (nolock)  on a.payingcapacityid=pc.id
		 inner join [state] st with (nolock)  on ad.stateid=st.id
		 left join  [state] st1 with (nolock)  on ad.prevstateid=st1.id
		 left join [state] st2 with (nolock)  on c.DriverLicenseStateID=st2.id
		 inner join streettype stt with (nolock)  on ad.streettypeid=stt.id
		 inner join housingtype ht with (nolock)  on pc.housingtypeid=ht.id
		 inner join lot lot with (nolock)  on ca.lotid=lot.id
		 inner join DataviewModelScore dv with (nolock)  on a.Dataviewmodelscoreid=dv.id
 
 
	 WHERE 
	 --ca.datemodified>DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0) and 
	 ca.datemodified > @ProcessLastDataTime    -- BETWEEN '2019-10-02' AND '2019-10-03' AND
	 AND creditappstatusid >= 4 

 UNION ALL
  
  SELECT 
		 st.StateAbbreviation as jurisdictionState,
		 upper(isNull(c.firstName,'')) as firstName,
		 upper(isNull(c.middleName,'')) as middleName,
		 upper(isNull(c.lastName,'')) as lastName,
		 upper(ad.HouseNumber + ' '+ rtrim(ltrim(ad.StreetName)) +' '+rtrim(ltrim(stt.StreetTypeName))) as StreetAddress1,
		 upper(isNull(ad.city,'')) as city1,
		 isNull(st.StateAbbreviation ,'') as state1,
		 substring(ad.postalcode,1,5) as postalCode1,
		 country1='USA',
		 ad.totalmonths as monthsAtResidence1,
		 ht.HousingTypeName as residenceStatus1,
		 'TRUE' as isCurrentResidence,
		 upper(rtrim(ltrim(isNull(isNull(ad.PrevStreetName,ad.PrevAddressLine),'')))) as streetAddress2,
		 upper(isNull(ad.prevcity,'')) as city2,
		 isNull(st1.StateAbbreviation ,'') as state2,
		 isNull(substring(ad.prevpostalcode,1,5),'') as postalCode2,
		 country2='USA',
		 ht.HousingTypeName as residenceStatus2,
		isNull(replace(replace(replace(replace(c.mobilephone,'-',''),'(',''),')',''),' ',''),'') as mobilePhone,
		isNull(replace(replace(replace(replace(c.homephone,'-',''),'(',''),')',''),' ',''),'') as homePhone, 
		isNull(replace(replace(replace(replace(e.workphone,'-',''),'(',''),')',''),' ',''),'') as workPhone,
		upper(isNull(c.emailaddress,'')) as email,
		case when c.dateofbirth is null then ''
		else convert(varchar(10),c.dateofbirth,121)
		end as birthday,
		ca.id as applicationID,
		c.ssn as SSN ,
		isNull(c.DriverLicenseNumber,'') as driverLicenseNumber,
		isNull(st2.StateAbbreviation,'') as issueState,
		isNull(case when isNull(cast(c.DriverlicenseExpirationDate as varchar(10)),'')='1900-01-01' then '' else cast(c.DriverlicenseExpirationDate as varchar(10)) end,'')  as  driverlicenseExpirationDate,
		upper(isNull(e.name,'')) as employerName,
		isNull(replace(replace(replace(e.workphone,'-',''),'(',''),')',''),'') as employerPhone,
		upper(isNull(e.Position,'')) as jobTitle,
		'TRUE' as isCurrentlyEmployed,
		s.netincome as monthlyIncomeNet,
		e.totalmonths as employmentMonthCount,
		case pc.paymenttypeid when '1' then '52' when '2' then '26' when '3' then '24' else '12' end as paymentsPerYear,
		convert(varchar(10),ca.datecreated,121) as dateOriginated,
		'60' as term,
		c.id as clientCustomerId,
		ca.id as clientLoanReferenceId,
		a.id as clientApplicationId,
		s.maxpurchaseprice as maxPurchasePrice, 
		case 
			when isNull(s.AdjustedMonthlyPayment,0)>0 then s.AdjustedMonthlyPayment 
			else s.MaxMonthlyPayment 
		end as maxMonthlyPayment,
		s.MinDownPaymentPercent AS minDownPaymentPercent,
		s.maxpurchaseprice as principalAmount,
		s.irate as annualInterestRate,
		upper(lot.LotName) as lotName,
		lot.streetAddress,
		lot.city,
		lot.postalCode,
		lot.[state],
		dv.outputxml,
		ca.datemodified AS dateModified
 
	FROM
	 CreditScoreApplication ca with (nolock) 
	 inner join Application a with (nolock)  on ca.cobuyerapplicationid=a.id
	 inner join Score s with (nolock)  on ca.primaryscoreid=s.id
	 inner join Customer c with (nolock)  on a.customerid=c.id
	 inner join Address ad  with (nolock) on a.addressid=ad.id
	 inner join employment e with (nolock)  on a.employmentid=e.id
	 inner join payingcapacity pc with (nolock)  on a.payingcapacityid=pc.id
	 inner join [state] st with (nolock)  on ad.stateid=st.id
	 left join  [state] st1 with (nolock)  on ad.prevstateid=st1.id
	 left join [state] st2 with (nolock)  on c.DriverLicenseStateID=st2.id
	 inner join streettype stt with (nolock)  on ad.streettypeid=stt.id
	 inner join housingtype ht with (nolock)  on pc.housingtypeid=ht.id
	 inner join lot lot with (nolock)  on ca.lotid=lot.id
	 inner join DataviewModelScore dv with (nolock)  on a.Dataviewmodelscoreid=dv.id
 
	WHERE 
	 --ca.datemodified > DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0) and creditappstatusid >= 4 ;
	 --ca.datemodified = '2019-12-14'  and 
	 ca.datemodified > @ProcessLastDataTime    -- BETWEEN '2019-10-02' AND '2019-10-03' AND
	 AND creditappstatusid >= 4 
	 ) b ORDER BY dateModified ASC;


END
GO
/****** Object:  StoredProcedure [dbo].[TrustScienceGetBatchToProcessForSelectedDates]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =========================================================================================================================
-- Author:		Sam Aloni
-- Create date: 10/04/2020
-- Description:	Collect the batch of applications to process since the last time we processed the batch (Once an hour) 
-- EXEC TrustScienceGetBatchToProcessForSelectedDates;
-- =========================================================================================================================
create PROCEDURE [dbo].[TrustScienceGetBatchToProcessForSelectedDates]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	-- find the list date we processed
	DECLARE @ProcessLastDataTime datetime;

	SELECT TOP 1 @ProcessLastDataTime = [LastItemDateTime] FROM [dbo].[TrustScienceScoreProcessingResult] ORDER BY [LastItemDateTime] DESC;

	IF @ProcessLastDataTime IS NULL
	  BEGIN
		--SET @ProcessLastDataTime = DATEADD(HOUR, -1, GETDATE());
		SET  @ProcessLastDataTime =  CONVERT(date, getdate())
	  END

	-- *SELECT @ProcessLastDataTime*

	SELECT * FROM
	(
	SELECT DISTINCT
		 st.StateAbbreviation as jurisdictionState,
		 upper(isNull(c.firstName,'')) as firstName,
		 upper(isNull(c.middleName,'')) as middleName,
		 upper(isNull(c.lastName,'')) as lastName,
		 upper(ad.HouseNumber + ' '+ rtrim(ltrim(ad.StreetName)) +' '+rtrim(ltrim(stt.StreetTypeName))) as StreetAddress1,
		 upper(isNull(ad.city,'')) as City1,
		 isNull(st.StateAbbreviation ,'') as State1,
		 substring(ad.postalcode,1,5) as PostalCode1,
		 country1='USA',
		 ad.totalmonths as monthsAtResidence1,
		 ht.HousingTypeName as residenceStatus1,
		 'TRUE' as isCurrentResidence,
		 upper(rtrim(ltrim(isNull(isNull(ad.PrevStreetName,ad.PrevAddressLine),'')))) as StreetAddress2,
		 upper(isNull(ad.prevcity,'')) as City2,
		 isNull(st1.StateAbbreviation ,'') as State2,
		 isNull(substring(ad.prevpostalcode,1,5),'') as PostalCode2,
		 country2='USA',
		 ht.HousingTypeName as residenceStatus2,
		isNull(replace(replace(replace(replace(c.mobilephone,'-',''),'(',''),')',''),' ',''),'') as mobilePhone,
		isNull(replace(replace(replace(replace(c.homephone,'-',''),'(',''),')',''),' ',''),'') as homePhone, 
		isNull(replace(replace(replace(replace(e.workphone,'-',''),'(',''),')',''),' ',''),'') as workPhone,
		upper(isNull(c.emailaddress,'')) as email,
		case when c.dateofbirth is null then ''
		else convert(varchar(10),c.dateofbirth,121)
		end as birthday,
		ca.id as applicationID,
		c.ssn as SSN ,
		upper(isNull(e.name,'')) as employerName,
		isNull(replace(replace(replace(e.workphone,'-',''),'(',''),')',''),'') as employerPhone,
		upper(isNull(e.Position,'')) as jobTitle,
		'TRUE' as isCurrentlyEmployed,
		s.netincome as monthlyIncomeNet,
		e.totalmonths as employmentMonthCount,
		case pc.paymenttypeid when '1' then '52' when '2' then '26' when '3' then '24' else '12' end as paymentsPerYear,
		convert(varchar(10),ca.datecreated,121) as dateOriginated,
		c.id as clientCustomerId,
		ca.id as clientLoanReferenceId,
		a.id as clientApplicationId,
		s.maxpurchaseprice as principalAmount, 
		s.irate as annualInterestRate,
		lot.LotName,
		lot.streetAddress,
		lot.city,
		lot.postalCode,
		lot.state,
		dv.outputxml,
		ca.datemodified AS dateModified
 
	FROM 
		 CreditScoreApplication ca with (nolock) 
		 inner join [Application] a with (nolock)  on ca.primarybuyerapplicationid=a.id
		 inner join Score s with (nolock)  on ca.primaryscoreid=s.id
		 inner join Customer c with (nolock)  on a.customerid=c.id
		 inner join [Address] ad with (nolock)  on a.addressid=ad.id
		 inner join employment e with (nolock)  on a.employmentid=e.id
		 inner join payingcapacity pc with (nolock)  on a.payingcapacityid=pc.id
		 inner join [state] st with (nolock)  on ad.stateid=st.id
		 left join  [state] st1 with (nolock)  on ad.prevstateid=st1.id
		 inner join streettype stt with (nolock)  on ad.streettypeid=stt.id
		 inner join housingtype ht with (nolock)  on pc.housingtypeid=ht.id
		 inner join lot lot with (nolock)  on ca.lotid=lot.id
		 inner join DataviewModelScore dv with (nolock)  on a.Dataviewmodelscoreid=dv.id
 
 
	 WHERE 
	 --ca.datemodified>DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0) and 
	 -- ca.datemodified > @ProcessLastDataTime    -- BETWEEN '2019-10-02' AND '2019-10-03' AND
	 ca.datemodified BETWEEN '2019-10-01' AND '2019-10-03'
	 AND creditappstatusid >= 4 

 UNION ALL
  
  SELECT 
	 st.StateAbbreviation as jurisdictionState,
	 upper(isNull(c.firstName,'')) as firstName,
	 upper(isNull(c.middleName,'')) as middleName,
	 upper(isNull(c.lastName,'')) as lastName,
	 upper(ad.HouseNumber + ' '+ rtrim(ltrim(ad.StreetName)) +' '+rtrim(ltrim(stt.StreetTypeName))) as StreetAddress1,
	 upper(isNull(ad.city,'')) as City1,
	 isNull(st.StateAbbreviation ,'') as State1,
	 substring(ad.postalcode,1,5) as PostalCode1,
	 country1='USA',
	 ad.totalmonths as monthsAtResidence1,
	 ht.HousingTypeName as residenceStatus1,
	 'TRUE' as isCurrentResidence,
	 upper(rtrim(ltrim(isNull(isNull(ad.PrevStreetName,ad.PrevAddressLine),'')))) as StreetAddress2,
	 upper(isNull(ad.prevcity,'')) as City2,
	 isNull(st1.StateAbbreviation ,'') as State2,
	 isNull(substring(ad.prevpostalcode,1,5),'') as PostalCode2,
	 country2='USA',
	 ht.HousingTypeName as residenceStatus2,
	isNull(replace(replace(replace(replace(c.mobilephone,'-',''),'(',''),')',''),' ',''),'') as mobilePhone,
	isNull(replace(replace(replace(replace(c.homephone,'-',''),'(',''),')',''),' ',''),'') as homePhone, 
	isNull(replace(replace(replace(replace(e.workphone,'-',''),'(',''),')',''),' ',''),'') as workPhone,
	upper(isNull(c.emailaddress,'')) as email,
	case when c.dateofbirth is null then ''
	else convert(varchar(10),c.dateofbirth,121)
	end as birthday,
	ca.id as applicationID,
	c.ssn as SSN ,
	upper(isNull(e.name,'')) as employerName,
	isNull(replace(replace(replace(e.workphone,'-',''),'(',''),')',''),'') as employerPhone,
	upper(isNull(e.Position,'')) as jobTitle,
	'TRUE' as isCurrentlyEmployed,
	s.netincome as monthlyIncomeNet,
	e.totalmonths as employmentMonthCount,
	case pc.paymenttypeid when '1' then '52' when '2' then '26' when '3' then '24' else '12' end as paymentsPerYear,
	convert(varchar(10),ca.datecreated,121) as dateOriginated,
	c.id as clientCustomerId,
	ca.id as clientLoanReferenceId,
	a.id as clientApplicationId,
	s.maxpurchaseprice as principalAmount, 
	s.irate as annualInterestRate,
	lot.LotName,
	lot.streetAddress,
	lot.city,
	lot.postalCode,
	lot.state,
	dv.outputxml,
	ca.datemodified AS dateModified
 
	FROM
	 CreditScoreApplication ca with (nolock) 
	 inner join Application a with (nolock)  on ca.cobuyerapplicationid=a.id
	 inner join Score s with (nolock)  on ca.primaryscoreid=s.id
	 inner join Customer c with (nolock)  on a.customerid=c.id
	 inner join Address ad  with (nolock) on a.addressid=ad.id
	 inner join employment e with (nolock)  on a.employmentid=e.id
	 inner join payingcapacity pc with (nolock)  on a.payingcapacityid=pc.id
	 inner join state st with (nolock)  on ad.stateid=st.id
	 left join  state st1 with (nolock)  on ad.prevstateid=st1.id
	 inner join streettype stt with (nolock)  on ad.streettypeid=stt.id
	 inner join housingtype ht with (nolock)  on pc.housingtypeid=ht.id
	 inner join lot lot with (nolock)  on ca.lotid=lot.id
	 inner join DataviewModelScore dv with (nolock)  on a.Dataviewmodelscoreid=dv.id
 
	WHERE 
	 --ca.datemodified > DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0) and creditappstatusid >= 4 ;
	 --ca.datemodified = '2019-12-14'  and 
	 --ca.datemodified > @ProcessLastDataTime    -- BETWEEN '2019-10-02' AND '2019-10-03' AND
	 ca.datemodified BETWEEN '2019-10-01' AND '2019-10-03'
	 AND creditappstatusid >= 4 
	 ) b ORDER BY dateModified ASC;


END
GO
/****** Object:  StoredProcedure [dbo].[TrustScienceGetFailedBatchToReprocess]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =========================================================================================================================
-- Author:		Sam Aloni
-- Create date: 10/05/2020
-- Description:	Collect the failed batch of applications to re-process
-- EXEC TrustScienceGetFailedBatchToReprocess;
-- =========================================================================================================================
CREATE PROCEDURE [dbo].[TrustScienceGetFailedBatchToReprocess]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- find the list date we processed
	SELECT [ID], [RequestID] 
	FROM [dbo].[TrustScienceScore] 
	WHERE [CallStatus] = 'Bad Request' OR QualifierCode1 = 'D14';

END
GO
/****** Object:  StoredProcedure [dbo].[TrustScienceGetlogIdByReqId]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =================================================================================================
-- Author:		Sam Aloni
-- Create date: 10-22-2020
-- Description:	Get the LogID by the RequestID
-- EXEC TrustScienceGetlogIdByReqId 'ec463890-147b-11eb-a4cc-0533a5438d6';
-- =================================================================================================
create PROCEDURE [dbo].[TrustScienceGetlogIdByReqId]
		@RequestID varchar(100)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @logID int = -1;

	SELECT
		@logID = ID
	FROM 
		[dbo].[TrustScienceScore]
	WHERE 
		[RequestID] = @RequestID;


	SELECT @logID;
END
GO
/****** Object:  StoredProcedure [dbo].[TrustScienceGetNormaliazedIncome]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =========================================================================================================================
-- Author:		Sam Aloni
-- Create date: 10/29/2020
-- Description:	Collect the batch of applications to process since the last time we processed the batch (Once an hour) 
-- EXEC TrustScienceGetApplicationDetail 294084;
-- =========================================================================================================================
create PROCEDURE [dbo].[TrustScienceGetNormaliazedIncome]
	@ApplicationID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT 
		CAST ((pc.PeriodPaycheck * st.NetIncomeMultiplier) AS int)
	FROM Application a
	INNER JOIN PayingCapacity pc ON a.PayingCapacityID = pc.Id
	INNER JOIN SalaryType st ON pc.PaymentTypeID = st.PaymentTypeID
	WHERE
		a.Id = @ApplicationID;


END
GO
/****** Object:  StoredProcedure [dbo].[TrustScienceSaveCreateFullScoringReqResp]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =================================================================================================
-- Author:		Sam Aloni
-- Create date: 11-02-2020
-- Description:	Save call request and response to Trust Science Create Full Scoring Request
-- =================================================================================================
create PROCEDURE [dbo].[TrustScienceSaveCreateFullScoringReqResp]
		@CustomerID int,
		@ApplicationID int,
		@Request varchar(max),
		@Response varchar(max),
		@CallStatus varchar(100),
		@RequestID varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[TrustScienceScore]
		 (
			    [CustomerID]
			   ,[ApplicationID]
			   ,[Request]
			   ,[CreateFullScoringResp]
			   ,[CreateDate]
			   ,[CallStatus]
			   ,[RequestID]
		 )
		 OUTPUT Inserted.Id
	VALUES
		 (
		 		@CustomerID
			   ,@ApplicationID
			   ,@Request
			   ,@Response
			   ,GETDATE()
			   ,@CallStatus
			   ,@RequestID
		 )

		
END
GO
/****** Object:  StoredProcedure [dbo].[TrustScienceSaveGetScoringReportResp]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =================================================================================================
-- Author:		Sam Aloni
-- Create date: 09-30-2020
-- Description:	Save call request and response to Trust Science Create Full Scoring Request
-- =================================================================================================
create PROCEDURE [dbo].[TrustScienceSaveGetScoringReportResp]
		@LogID int,
		@RequestID varchar(100),
		@Score int,
		@QualifierCode1 varchar(20),
		@QualifierCodeDescription1 varchar(100),
		@QualifierCode2 varchar(20),
		@QualifierCodeDescription2 varchar(100),
		@QualifierCode3 varchar(20),
		@QualifierCodeDescription3 varchar(100),
		@QualifierCode4 varchar(20),
		@QualifierCodeDescription4 varchar(100),

		@ScoreReasonCode1 varchar(20),
		@ScoreReasonDescription1 varchar(100),
		@ScoreReasonCode2 varchar(20),
		@ScoreReasonDescription2 varchar(100),
		@ScoreReasonCode3 varchar(20),
		@ScoreReasonDescription3 varchar(100),
		@ScoreReasonCode4 varchar(20),
		@ScoreReasonDescription4 varchar(100),

		@ScoringDetailsURL varchar(250),
		@Response varchar(max),
		@CallStatus varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[TrustScienceScore]
   SET 
       [Score] = @Score
      ,[RequestID] = @RequestID
      ,[QualifierCode1] = @QualifierCode1
      ,[QualifierCodeDescription1] = @QualifierCodeDescription1
      ,[QualifierCode2] = @QualifierCode2
      ,[QualifierCodeDescription2] = @QualifierCodeDescription2
      ,[QualifierCode3] = @QualifierCode3
      ,[QualifierCodeDescription3] = @QualifierCodeDescription3
      ,[QualifierCode4] = @QualifierCode4
      ,[QualifierCodeDescription4] = @QualifierCodeDescription4

	  ,[ScoreReasonCode1] = @ScoreReasonCode1
	  ,[ScoreReasonDescription1] = @ScoreReasonDescription1
	  ,[ScoreReasonCode2] = @ScoreReasonCode2
	  ,[ScoreReasonDescription2] = @ScoreReasonDescription2
	  ,[ScoreReasonCode3] = @ScoreReasonCode3
	  ,[ScoreReasonDescription3] = @ScoreReasonDescription3
	  ,[ScoreReasonCode4] = @ScoreReasonCode4
	  ,[ScoreReasonDescription4] = @ScoreReasonDescription4

      ,[ScoringDetailsURL] = @ScoringDetailsURL
      ,[Response] = @Response
      ,[CallStatus] = @CallStatus
	  ,[RecivedReportOn] = GETDATE()
	WHERE ID = @LogID;

	DECLARE @ApplicationID int = 0

	SELECT @ApplicationID = ApplicationID FROM TrustScienceScore WHERE [RequestID] = @RequestID;

    -- insert the ref to the item into Application table
	UPDATE
		[dbo].[Application]
	SET
		[TrustScienceScoreID] = @LogID
	WHERE
		[Id] = @ApplicationID;

END
GO
/****** Object:  StoredProcedure [dbo].[TrustScienceScoreGetLastProcessingDateTime]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ===========================================================================
-- Author:		Sam Aloni
-- Create date: 10/01/2020
-- Description:	return the last item Processing time
--				so we can collect all the items greater than that datetime
-- EXEC TrustScienceScoreGetLastProcessingDateTime;
-- ===========================================================================
create PROCEDURE [dbo].[TrustScienceScoreGetLastProcessingDateTime] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT TOP 1 LastItemDateTime
	FROM TrustScienceScoreProcessingResult
	ORDER BY ID DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[TrustScienceScoreSaveProcessingResult]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =====================================================================
-- Author:		Sam Aloni
-- Create date: 10/01/2020
-- Description:	Save last processing information to log table
-- EXEC TrustScienceScoreSaveProcessingResult '2020-09-28', 100, 92, 8 , 7, '2020-09-29', '2020-09-30';
-- SELECT * FROM [TrustScienceScoreProcessingResult];
-- =====================================================================
create PROCEDURE [dbo].[TrustScienceScoreSaveProcessingResult]
	@LastItemDateTime datetime,
	@TotalItemCount int ,
	@SuccessfulItemCount int ,
	@FailedItemCount int,
	@ProcessingErrorItemCount int,
	@StartDateTime datetime, 
	@EndDateTime datetime

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [TrustScienceScoreProcessingResult]
        (
			 [LastItemDateTime]
			,[TotalItemCount]
			,[SuccessfulItemCount]
			,[FailedItemCount]
			,[ProcessingErrorItemCount]
			,[StartDateTime]
			,[EndDateTime]
		)
     VALUES
		(
			@LastItemDateTime,
			@TotalItemCount,
			@SuccessfulItemCount,
			@FailedItemCount,
			@ProcessingErrorItemCount,
			@StartDateTime,
			@EndDateTime
		)

END
GO
/****** Object:  StoredProcedure [dbo].[TrustScienceUpdateRetryCount]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =================================================================================================
-- Author:		Sam Aloni
-- Create date: 10-06-2020
-- Description:	Update the Get Report retry count
-- =================================================================================================
create PROCEDURE [dbo].[TrustScienceUpdateRetryCount]
		@ID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @currentCount int; 

	SELECT @currentCount = [RetryCount] FROM [TrustScienceScore] WHERE ID = @ID;

	IF @currentCount IS NULL
	 BEGIN
		SET @currentCount = 0;
	 END
	-- update the retry count
	UPDATE [dbo].[TrustScienceScore]
	SET 
		[RetryCount] = @currentCount + 1
	WHERE ID = @ID;



END
GO
/****** Object:  StoredProcedure [logs].[GetCurrentActiveFlowId]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
------------------------------------------------------------------------------------------------------------------------
-- Date Created: Saturday, November 15, 2020
-- Created By:   Sam Aloni
-- Get the ID of the current active Application Flow
--  Sample call:
--  DECLARE @ApplicationFlowId int;  
--  EXEC [logs].[GetCurrentActiveFlowId] @ApplicationFlowId output;
--  SELECT @ApplicationFlowId;
------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [logs].[GetCurrentActiveFlowId]
	@ApplicationFlowId int OUTPUT
AS
SELECT @ApplicationFlowId = [Id]
FROM [dbo].[ApplicationFlow]
WHERE [IsActive] = 1;

GO
/****** Object:  StoredProcedure [logs].[GetStepsByFlowId]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
------------------------------------------------------------------------------------------------------------------------
-- Date Created: Saturday, November 15, 2020
-- Created By:   Sam Aloni
-- Get the ID of the current active Application Flow
--  Sample call:  
--  EXEC [logs].[GetStepsByFlowId] @ApplicationFlowId;
------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [logs].[GetStepsByFlowId]
	@ApplicationFlowId int OUTPUT
AS

SELECT * 
FROM [dbo].[ApplicationStep]
WHERE [ApplicationFlowId] = @ApplicationFlowId;


GO
/****** Object:  StoredProcedure [logs].[MarkStepAsCompleted]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =================================================================================================
-- Author:		Sam Aloni
-- Create date: 11-20-2020
-- Description:	Save step as completed in table [logs].[ApplicationFlowStepResult]
-- EXEC [logs].[MarkStepAsCompleted] ???;
-- =================================================================================================
CREATE PROCEDURE [logs].[MarkStepAsCompleted]
		@ApplicationFlowStepResultID int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE 
		[logs].[ApplicationFlowStepResult]
	SET
		[IsCompleted] = 1
	WHERE
		[Id] = @ApplicationFlowStepResultID;

END
GO
/****** Object:  StoredProcedure [logs].[SaveOriginalApp]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =================================================================================================
-- Author:		Sam Aloni
-- Create date: 11-12-2020
-- Description:	Save user original application
-- EXEC [logs].[SaveOriginalApp] 'test #1';
-- =================================================================================================
CREATE PROCEDURE [logs].[SaveOriginalApp]
		@FirstName varchar(50),
		@LastName varchar(50),
		@PhoneNumber varchar(50),
		@Email varchar(50),
		@Last4Ssn varchar(50),
		@OriginalApp varchar(max)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @ClientApplicationId int = -1, @ApplicationFlowId int = -1;
	DECLARE @IdentityOutput TABLE ( ID int );

	SELECT @ApplicationFlowId = Id FROM [dbo].[ApplicationFlow] WHERE [IsActive] = 1;

	INSERT INTO [logs].[ClientApplication]
		 (
			[FirstName]
           ,[LastName]
           ,[PhoneNumber]
           ,[Email]
           ,[Last4Ssn]
           ,[OriginalApp]
		   ,ApplicationFlowId
		 )
		 OUTPUT inserted.Id INTO @IdentityOutput
	VALUES
		 (
			@FirstName
           ,@LastName
           ,@PhoneNumber
           ,@Email
           ,@Last4Ssn
           ,@OriginalApp
		   ,@ApplicationFlowId
		 );

	-- return the [logs].[ClientApplication] last inserted Identity
	SELECT @ClientApplicationId = (SELECT ID FROM @IdentityOutput);

	-- load current flow steps into ApplicationFlowStep
	INSERT INTO [logs].[ApplicationFlowStepResult]
	SELECT 
			 @ClientApplicationId
			,[Id]
			,0 
			,[StepOrder]
			,NULL			-- all nulls are respone and request data
			,NULL
			,NULL
			,NULL
			,GETDATE()
		 FROM
			[dbo].[ApplicationFlowStep]
		 WHERE 
			[ApplicationFlowId] = @ApplicationFlowId;

	SELECT @ClientApplicationId;

END
GO
/****** Object:  StoredProcedure [logs].[UpdateLogIds]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
------------------------------------------------------------------------------------------------------------------------
-- Date Created: Saturday, November 14, 2020
-- Created By:   Sam Aloni
-- Update table ClientApplication with IDs
-- exec [logs].[UpdateLogIds] 17,1,2,3,4,5,6;
------------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [logs].[UpdateLogIds]
	@Id int, 
	@CreditScoreAppId int, 
	@ApplicationId int, 
	@PayingCapacityId int, 
	@AddrId int, 
	@CustId int ,
	@EmploymentId int

AS

BEGIN TRY
	UPDATE [logs].[ClientApplication] 
	SET
		[CreditScoreAppId] = @CreditScoreAppId,
		[ApplicationId] = @ApplicationId,
		[PayingCapacityId] = @PayingCapacityId,
		[AddrId] = @AddrId,
		[CustId] = @CustId,
		[EmploymentId] = @EmploymentId
	WHERE
		[Id] = @Id
END TRY
BEGIN CATCH  
      SELECT 'error';  
    SELECT  ERROR_LINE() AS [Error_Line],
        ERROR_MESSAGE() AS [Error_Message],
        ERROR_NUMBER() AS [Error_Number],
        ERROR_SEVERITY() AS [Error_Severity],
        ERROR_PROCEDURE() AS [Error_Procedure];

END CATCH 
GO
/****** Object:  StoredProcedure [pointPredictive].[SaveReqResp]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==================================================================
-- Author:		Sam Aloni / Travis
-- Create date: 11/21/2020
-- Description:	Save Point Predictive Request and response data
--
/*
	EXEC PointPredictiveSaveReqResp 206354,'reqString', 'respString', 
		'ssnString', 1,'FraudEpdReportLink','DealerRiskReportLink','456',
		'1','22','333','Clear req','*********',1,'Comment';
*/
-- ==================================================================
CREATE PROCEDURE [pointPredictive].[SaveReqResp] 
	@creditID int,								-- it is ApplicationID
	@reqString varchar(MAX),
	@respString varchar(MAX),
	@ssnString varchar(MAX),
	@completed bit,
	@FraudEpdReportLink varchar(MAX),
	@DealerRiskReportLink varchar(MAX),
	@FraudScore varchar(25),
	@FraudReasonCode1 [varchar](10),
	@FraudReasonCode2 [varchar](10),
	@FraudReasonCode3 [varchar](10),
	@CleanReq varchar(MAX),
	@ErrorMsg varchar(MAX),
	@CreatedBy int ,
	@Comment varchar(MAX)


AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	-- collect Status information and load into PP_return table
	DECLARE @GradeValue int, @StatusID int, @fraudScoreInt int;

	--Added by TRM
	Declare @ScoreId int
	Declare @ScoreIDJoint int
	Declare @DPReduction float
	Declare @DPPct float
	Declare @DPJoint float
	Declare @AdjDPPct float
	Declare @AdjDPPctJoint float
	Declare @Previous int = 0

	SET @fraudScoreInt = CAST(@FraudScore AS int);

	SELECT TOP 1
		@GradeValue = gss.GradeValue,
		@StatusID = gfm.UWStatusID
	FROM 
		dbo.creditscoreapplication ca
	LEFT JOIN
		[dbo].[Score] s ON ca.PrimaryScoreID = s.Id
	LEFT JOIN
		[dbo].[GradeScoreSegment] gss on s.GradeScoreSegmentID = gss.id
	LEFT JOIN
		[dbo].[Grade_FraudMatrix] gfm ON gss.GradeValue = gfm.GradeValue
	WHERE 
		ca.Id = @creditID 
		AND ca.CreditAppStatusID > 7 
		AND @fraudScoreInt BETWEEN gfm.FraudScoreLow AND gfm.FraudScoreHigh

	-- SELECT @GradeValue , @StatusID

	INSERT INTO [dbo].[PP_Return]
           ([CreditId]
           ,[Completed]
           ,[CreatedDate]
           ,[Request]
           ,[Response]
           ,[SsnRespone]
           ,[Errors]
		   ,[FraudEpdReportLink]
		   ,[DealerRiskReportLink]
		   ,[FraudScore]
		   ,[FraudReasonCode1]
		   ,[FraudReasonCode2]
		   ,[FraudReasonCode3]
		   ,[CleanRequest]
		   ,[CreatedBy]
		   ,[GradeValue]
		   ,[StatusID]
		   )
		   OUTPUT Inserted.Id
		 VALUES
			(@creditID
			,@completed
			,GETDATE()
			,@reqString
			,@respString
			,@ssnString
			,@ErrorMsg
			,@FraudEpdReportLink
			,@DealerRiskReportLink
			,@FraudScore
			,@FraudReasonCode1
			,@FraudReasonCode2
			,@FraudReasonCode3
			,@CleanReq
			,@CreatedBy
			,@GradeValue
			,@StatusID
			)

	--------------------------------------------------------------------------------------------
	-- Save the result to the the flow step log table [logs].[ApplicationFlowStepResult] 
	--------------------------------------------------------------------------------------------
	DECLARE @ApplicationFlowStepResultID int = 0

	SELECT 
		@ApplicationFlowStepResultID = afsr.id
	FROM
		[logs].[ApplicationFlowStepResult] afsr
	INNER JOIN
		[logs].[ClientApplication] ca ON afsr.ClientApplicationId = ca.id
	INNER JOIN
		[dbo].[ApplicationFlowStep] afs ON afsr.ApplicationFlowStepId = afs.Id
	WHERE
		ca.ApplicationId = @creditID AND
		afs.StepName = 'Point Predictive';

	UPDATE
		[logs].[ApplicationFlowStepResult]
	SET
		[IsCompleted] = 1
	WHERE
		[Id] = @ApplicationFlowStepResultID;


	select @ScoreID=s.id,@DPReduction=m.DPReduction,@DPPct=s.MinDownPaymentPercent,@ScoreIdJoint=js.id,@DPJoint=js.MinDownPaymentPercent From 
	pp_return pp with (nolock)
	inner join CreditScoreApplication ca with (nolock) on pp.CreditId=ca.id
	inner join Score s with (nolock) on ca.PrimaryScoreID=s.id
	left join Score js on ca.JointScoreID=js.id
	inner join [Grade_FraudMatrix] m with (nolock) on pp.GradeValue=m.GradeValue and pp.StatusID=m.UWStatusID
	where ca.id=@creditID

	if @DPReduction>0
		begin
	
		insert into TLog values (0,getdate(),'StartProc')


		insert into TLog values (@creditID,getdate(),'Credit ID'+convert(varchar(10),@creditID))

			if @DPJoint>0
				begin
				
					select  @Previous=ID from dpchanges_hist where scoreid=@ScoreId

					insert into TLog values (@creditID,getdate(),'Previous ID='+convert(varchar(10),@Previous))
				
					if @Previous=0
						begin
						
							insert into TLog values (@creditID,getdate(),'In Joint')
							set @AdjDPPct=@DPPct-@DPReduction
							set @AdjDPPctJoint=@DPJoint-@DPReduction
				
							update score
							set MinDownPaymentPercent=@AdjDPPct,DateModified=GETDATE()
							where id=@ScoreId

							update score
							set MinDownPaymentPercent=@AdjDPPctJoint,DateModified=GETDATE()
							where id=@ScoreIDJoint

							insert into [dbo].[DPChanges_Hist] (CreditScoreID,ScoreID,JointScoreID,PrevDownPaymentPct,AdjDownPaymentPct,PrevDownPaymentPctJoint,AdjDownPaymentPctJoint,modifieddate)
							values (@creditID,@ScoreId,@ScoreIDJoint,@DPPct,@AdjDPPct,@DPJoint,@AdjDPPctJoint,getdate())

						
							insert into comment (Text,DateCreated,CreatedBy,CreditScoreApplicationID,DateModified,ModifiedBy)
							select  'DP % reduced to :'+convert(varchar(5),(@AdjDPPct*100))+'%',getdate(),1,@creditID,getdate(),1 

						END
			
				end
			else
				begin
					select  @Previous=ID from dpchanges_hist where scoreid=@ScoreId
				
					if @Previous=0
						begin
						   insert into TLog values (@creditID,getdate(),'In Single')
							set @AdjDPPct=@DPPct-@DPReduction 
				
							update score
							set MinDownPaymentPercent=@AdjDPPct,DateModified=GETDATE()
							where id=@ScoreId and id not in (select distinct scoreid from dpchanges_hist)

							insert into [dbo].[DPChanges_Hist] (CreditScoreID,ScoreID,PrevDownPaymentPct,AdjDownPaymentPct,modifieddate)
							values (@creditID,@ScoreId,@DPPct,@AdjDPPct,getdate()) 

							insert into comment (Text,DateCreated,CreatedBy,CreditScoreApplicationID,DateModified,ModifiedBy)
							select  'DP % reduced to :'+convert(varchar(5),(@AdjDPPct*100))+'%',getdate(),1,@creditID,getdate(),1 
						end 
				
				end
		end

END

GO
/****** Object:  StoredProcedure [pp].[GetApplicationByAppID]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*****************************************************************************************
*
*
*  EXEC [pp].[GetApplicationByAppID] 321818;
*****************************************************************************************/

CREATE PROCEDURE [pp].[GetApplicationByAppID]
	-- Add the parameters for the stored procedure here
	@applicationID int
AS

BEGIN
	SET NOCOUNT ON;

	DECLARE @CreditScoreApplicationID int = -1;

	SELECT @CreditScoreApplicationID = ca.Id
	FROM [dbo].[CreditScoreApplication] ca
	INNER JOIN 
		[dbo].[Application] a ON ca.PrimaryBuyerApplicationID = @applicationID
	WHERE a.Id = 321818

	--SELECT @CreditScoreApplicationID;

	--SELECT * FROM [dbo].[CreditScoreApplication] WHERE ID  = @CreditScoreApplicationID;

	SELECT 
		'2018.05' as InterfaceVersion,
		'USAUTO' as AccountIdentifier,
		'' as LenderIdentifier,
		ca.id as ApplicationIdentifier,
		convert(varchar(8),ca.datecreated,112) as ApplicationDate,
		'PENDING' as ApplicationStatus,
		lt.lotcode as	DealerIdentifier,

		upper(cu.firstname) as FirstName,
		upper(cu.lastname) as LastName,
		upper(ad.housenumber+ ' '+ad.streetname + ' '+st.streettypename) as StreetAddress,
		upper(ad.city) as City,
		upper(sta.Stateabbreviation) as State,
		ad.postalcode as Zip,
		'USA' as Country,
		isNull(replace(replace(replace(replace(Replace(cu.homephone,'(',''),')',''),'-',''),'.',''),' ','') ,'') as HomePhoneNumber,
		isNull( replace(replace(replace(replace(Replace(em.workphone,'(',''),')',''),'-',''),'.',''),' ','')  ,'')  as WorkPhoneNumber,
		isNull( replace(replace(replace(replace(Replace(cu.mobilephone,'(',''),')',''),'-',''),'.',''),' ',''),'') as CellPhoneNumber,
		isNull(upper(cu.emailaddress),'') as [Email],
		isNull(convert(varchar(8),cu.dateofbirth,112),'') as DateofBirth,
		 (cu.ssn)  as SSN,
		case when pc.housingtypeid='2' then 'O'
		else 'R'
		END AS RentOwn,
		'' as RentMortgage,
		ad.totalmonths as MonthsatResidence,
		isNull(upper(em.position),'') as Occupation,
		case pc.paymenttypeid
		when '1' then pc.periodpaycheck * 52
		when '2' then pc.periodpaycheck * 26
		when '3' then pc.periodpaycheck * 24
		else pc.periodpaycheck * 12
		end as AnnualIncome,
		'' as [SelfEmployed],
		isNull(upper(em.name),'') as EmployerName, 
		isNull(upper(em.fulladdress),'') as EmployerStreetAddress,
		'' as  EmployerCity,
		'' as EmployerState,
		'' as EmployerZIP,
		isNull(replace(replace(replace(replace(Replace(em.workphone,'(',''),')',''),'-',''),'.',''),' ',''),'')  as EmployerPhone,
		em.totalmonths as MonthsatEmployer,
		'0' as OtherBankRelationships,
		'' as CustomerSinceDate,
		case ca.clientancestorid
		when '3' then 'PREQUAL'
		WHEN '4' THEN 'PHONE'
		ELSE 'STORE'
		end as Channel ,
		--End PrimaryBorrower Info

		--Begin Loan Information
		sc.MaxFinance as LoanAmount,
		(sc.maxpurchaseprice*sc.mindownpaymentpercent) as TotalDownPayment,
		sc.availabledownpayment as CashDownPayment,
		'60' as Term,
		sc.pti*100 as [PaymentToIncomeRatio],
		--End Loan Information

		--Begin Credit Information PrimaryBorrower
		dv.beaconscore as CreditScore,
		'' as TimeinFile,
		'' as DebtToIncomeRatio,
		case when isNull(dv.INQ6_5,'')<0 then 0 else isNull(dv.INQ6_5,'') end as NumberofCreditInquiriesinprevioustwoweeks, 
		isNull(dv.HCACC_3,'') as HighestCreditLimitfromTradesinGoodStanding,
		isNull(isNull(dv.OPNSAT1,0)/nullif(dv.satrat1,0),0) as TotalNumberofTradeLines,

		case when (isNull(dv.OPNSAT1,0) + isNUll(dv.ACT6_1,0)) <0 then 0 else (isNull(dv.OPNSAT1,0) + isNUll(dv.ACT6_1,0)) end as NumberofOpenTradeLines,
		case when isNull(dv.pstdu_13,0) <0 then 0 else isNull(dv.pstdu_13,0)  end as NumberofPositiveAutoTrades	,
		case when isNull(dv.TSAT_11,'') <0 then 0 else isNull(dv.TSAT_11,'') end as NumberofMortgageTradeLines	,
		null as NumberofAuthorizedTradeLines	,
		case when DV.OLDAG_1>0 then convert(varchar(8),dateadd(MONTH,DV.OLDAG_1*-1, DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0)),112)
		else '' end as DateofOldestTradeLine,
		sc.MaxPurchasePrice as SalePrice,
		'' as YearofManufacture,
		'' as Make,
		'' as Model	,
		'' as VIN	,
		'USED' as NeworUsed,
		'' as Mileage,


		isNull(upper(cuc.firstname),'') as CobFirstName,
		isNull(upper(cuc.lastname),'') as CobLastName,
		isNull(upper(cad.housenumber+ ' '+cad.streetname + ' '+cst.streettypename),'') as CobStreetAddress,
		isNull(upper(cad.city),'') as CobCity,
		isNull(upper(csta.Stateabbreviation),'') as CobState,
		isNull(cad.postalcode,'') as CobZip,
		isNull(replace(replace(replace(Replace(cuc.homephone,'(',''),')',''),'-',''),'.',''),'') as CobHomePhoneNumber,
		isNull(replace(replace(replace(Replace(cem.workphone,'(',''),')',''),'-',''),'.',''),'')  as CobWorkPhoneNumber,
		isNull(replace(replace(replace(Replace(cuc.mobilephone,'(',''),')',''),'-',''),'.',''),'') as CobCellPhoneNumber,
		isNull(upper(cuc.emailaddress),'') as [CobEmail],
		isNull(convert(varchar(8),cuc.dateofbirth,112),'') as CobDateofBirth,
		isNull(
		(
			cast(
					(case cpc.paymenttypeid
					when '1' then cpc.periodpaycheck * 52
					when '2' then cpc.periodpaycheck * 26
					when '3' then cpc.periodpaycheck * 24
					else cpc.periodpaycheck * 12
					end) as varchar(10))),'') as CobAnnualIncome,
		'' as CobRelationship,
		isNull(cdv.beaconscore,'') as CobCreditScore,
		isNull(substring(convert(varchar(8),cu.dateofbirth,112),1,4),'') as PrimaryBorrowerYearofBirth,
		isNull(substring(convert(varchar(8),cuc.dateofbirth,112),1,4),'')  as CoBorrowerYearofBirth,
		'' as PrimaryBorrowerCreditScoreRange,
		'' as CoBorrowerCreditScoreRange,
		case when dv.satrat1 <0 then 0 else dv.satrat1 end  as UDF1,
		case when dv.mthdel1 <0 then 0 else dv.mthdel1 end as UDF2,
		case when dv.o24bl_6 <0 then 0 else dv.o24bl_6 end as UDF3,
		case when dv.plus90  <0 then 0 else dv.plus90 end as UDF4,
		case when dv.pstdu_1 <0 then 0 else dv.pstdu_1 end as UDF5,
		case when dv.ratio_1 <0 then 0 else dv.ratio_1 end as UDF6

	FROM dbo.creditscoreapplication ca with (nolock)
		--left join
		inner join
		dbo.lot lt with (nolock) on ca.lotid=lt.id
		inner join
		dbo.application a with (nolock) on ca.primarybuyerapplicationid=a.id
		left join
		dbo.application coa with (nolock) on ca.cobuyerapplicationid=coa.id
		inner join
		dbo.score sc with (nolock) on ca.primaryscoreid=sc.id
		left join
		dbo.score csc with (nolock) on ca.CoBuyerscoreid=csc.id
		left join
		dbo.dataviewmodelscore dv with (nolock) on a.dataviewmodelscoreid=dv.id
		left join
		dbo.dataviewmodelscore cdv with (nolock) on coa.dataviewmodelscoreid=cdv.id
		inner join
		dbo.customer cu with (nolock)  on a.customerid=cu.id
		left join
		dbo.customer cuc with (nolock) on coa.customerid=cuc.id
		inner join
		dbo.address ad with (nolock) on a.addressid=ad.id
		left join
		dbo.address cad with (nolock) on coa.addressid=cad.id
		inner join
		dbo.streettype st with (nolock) on ad.streettypeid=st.id
		left join
		dbo.streettype cst with (nolock) on cad.streettypeid=cst.id
		inner join
		dbo.state sta with (nolock) on ad.stateid=sta.id
		left join
		dbo.state csta with (nolock) on cad.stateid=csta.id
		inner join
		dbo.PayingCapacity pc with (nolock) on a.payingcapacityid=pc.id
		left join
		dbo.PayingCapacity cpc with (nolock) on coa.payingcapacityid=cpc.id
		inner join
		dbo.Employment em with (nolock) on a.employmentid=em.id
		left join
		dbo.Employment cem with (nolock) on coa.employmentid=cem.id
	WHERE 
		ca.id = @CreditScoreApplicationID -- AND ca.CreditAppStatusID > 7
END
GO
/****** Object:  StoredProcedure [trustScience].[__SaveReqRespToLog]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =================================================================================================
-- Author:		Sam Aloni
-- Create date: 11-20-2020
-- Description:	Save req/resp to log step table [logs].[ApplicationFlowStepResult]
-- currently not used.. we already have a loging table for Trust Science [dbo].[TrustScienceScore]
-- EXEC [trustScience].[SaveReqRespToLog] ';
-- =================================================================================================
CREATE PROCEDURE [trustScience].[__SaveReqRespToLog]
		@ApplicationFlowStepResultID int,
		@Req varchar(max),
		@Resp varchar(max),
		@Which int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;



END
GO
/****** Object:  StoredProcedure [trustScience].[GetApplicationDetail]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =========================================================================================================================
-- Author:		Travis / Sam Aloni
-- Create date: 10/13/2020
-- Description:	Collect the batch of applications to process since the last time we processed the batch (Once an hour) 
-- EXEC TrustScience 294084 ;
-- =========================================================================================================================
CREATE PROCEDURE [trustScience].[GetApplicationDetail]
	@ApplicationID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @CreditScoreApplicationID int = -1;
	-- get the CreditScoreApplicationID from the application ID
	SELECT @CreditScoreApplicationID = [Id]
	FROM [dbo].[CreditScoreApplication]
	WHERE [PrimaryBuyerApplicationID] = @ApplicationID;

	SELECT * FROM
	(
	SELECT DISTINCT
		 st.StateAbbreviation as jurisdictionState,
		 upper(isNull(c.firstName,'')) as firstName,
		 upper(isNull(c.middleName,'')) as middleName,
		 upper(isNull(c.lastName,'')) as lastName,
		 upper(ad.HouseNumber + ' '+ rtrim(ltrim(ad.StreetName)) +' '+rtrim(ltrim(stt.StreetTypeName))) as streetAddress1,
		 upper(isNull(ad.city,'')) as city1,
		 isNull(st.StateAbbreviation ,'') as state1,
		 substring(ad.postalcode,1,5) as postalCode1,
		 country1='USA',
		 ad.totalmonths as monthsAtResidence1,
		 ht.HousingTypeName as residenceStatus1,
		 'TRUE' as isCurrentResidence,
		 upper(rtrim(ltrim(isNull(isNull(ad.PrevStreetName,ad.PrevAddressLine),'')))) as streetAddress2,
		 upper(isNull(ad.prevcity,'')) as city2,
		 isNull(st1.StateAbbreviation ,'') as state2,
		 isNull(substring(ad.prevpostalcode,1,5),'') as postalCode2,
		 country2='USA',
		 ht.HousingTypeName as residenceStatus2,
		isNull(replace(replace(replace(replace(c.mobilephone,'-',''),'(',''),')',''),' ',''),'') as mobilePhone,
		isNull(replace(replace(replace(replace(c.homephone,'-',''),'(',''),')',''),' ',''),'') as homePhone, 
		isNull(replace(replace(replace(replace(e.workphone,'-',''),'(',''),')',''),' ',''),'') as workPhone,
		upper(isNull(c.emailaddress,'')) as email,
		case when c.dateofbirth is null then ''
		else convert(varchar(10),c.dateofbirth,121)
		end as birthday,
		ca.id as applicationID,
		c.ssn as SSN ,
		isNull(c.DriverLicenseNumber,'') as driverLicenseNumber,
		isNull(st2.StateAbbreviation,'') as issueState,
		isNull(case when isNull(cast(c.DriverlicenseExpirationDate as varchar(10)),'')='1900-01-01' then '' else cast(c.DriverlicenseExpirationDate as varchar(10)) end,'')  as  driverlicenseExpirationDate,
		upper(isNull(e.name,'')) as employerName,
		isNull(replace(replace(replace(e.workphone,'-',''),'(',''),')',''),'') as employerPhone,
		upper(isNull(e.Position,'')) as jobTitle,
		'TRUE' as isCurrentlyEmployed,
		s.netincome as monthlyIncomeNet,
		e.totalmonths as employmentMonthCount,
		case pc.paymenttypeid when '1' then '52' when '2' then '26' when '3' then '24' else '12' end as paymentsPerYear,
		convert(varchar(10),ca.datecreated,121) as dateOriginated,
		'60' as term,
		c.id as clientCustomerId,
		ca.id as clientLoanReferenceId,
		a.id as clientApplicationId,
		s.maxpurchaseprice as maxPurchasePrice, 
		case 
			when isNull(s.AdjustedMonthlyPayment,0) > 0 then s.AdjustedMonthlyPayment 
			else s.MaxMonthlyPayment 
		end as maxMonthlyPayment,
		s.MinDownPaymentPercent AS minDownPaymentPercent,
		s.maxpurchaseprice as principalAmount,
		s.irate as annualInterestRate,
		upper(lot.LotName) as lotName,
		lot.streetAddress,
		lot.city,
		lot.postalCode,
		lot.[state],
		dv.outputxml,
		ca.datemodified AS dateModified
 
	FROM 
		 CreditScoreApplication ca with (nolock) 
		 inner join [Application] a with (nolock)  on ca.primarybuyerapplicationid=a.id
		 inner join Score s with (nolock)  on ca.primaryscoreid=s.id
		 inner join Customer c with (nolock)  on a.customerid=c.id
		 inner join [Address] ad with (nolock)  on a.addressid=ad.id
		 inner join employment e with (nolock)  on a.employmentid=e.id
		 inner join payingcapacity pc with (nolock)  on a.payingcapacityid=pc.id
		 inner join [state] st with (nolock)  on ad.stateid=st.id
		 left join  [state] st1 with (nolock)  on ad.prevstateid=st1.id
		 left join [state] st2 with (nolock)  on c.DriverLicenseStateID=st2.id
		 inner join streettype stt with (nolock)  on ad.streettypeid=stt.id
		 inner join housingtype ht with (nolock)  on pc.housingtypeid=ht.id
		 inner join lot lot with (nolock)  on ca.lotid=lot.id
		 inner join DataviewModelScore dv with (nolock)  on a.Dataviewmodelscoreid=dv.id
 
	 WHERE 
		ca.id = @CreditScoreApplicationID

 UNION ALL
  
  SELECT 
		 st.StateAbbreviation as jurisdictionState,
		 upper(isNull(c.firstName,'')) as firstName,
		 upper(isNull(c.middleName,'')) as middleName,
		 upper(isNull(c.lastName,'')) as lastName,
		 upper(ad.HouseNumber + ' '+ rtrim(ltrim(ad.StreetName)) +' '+rtrim(ltrim(stt.StreetTypeName))) as StreetAddress1,
		 upper(isNull(ad.city,'')) as city1,
		 isNull(st.StateAbbreviation ,'') as state1,
		 substring(ad.postalcode,1,5) as postalCode1,
		 country1='USA',
		 ad.totalmonths as monthsAtResidence1,
		 ht.HousingTypeName as residenceStatus1,
		 'TRUE' as isCurrentResidence,
		 upper(rtrim(ltrim(isNull(isNull(ad.PrevStreetName,ad.PrevAddressLine),'')))) as streetAddress2,
		 upper(isNull(ad.prevcity,'')) as city2,
		 isNull(st1.StateAbbreviation ,'') as state2,
		 isNull(substring(ad.prevpostalcode,1,5),'') as postalCode2,
		 country2='USA',
		 ht.HousingTypeName as residenceStatus2,
		isNull(replace(replace(replace(replace(c.mobilephone,'-',''),'(',''),')',''),' ',''),'') as mobilePhone,
		isNull(replace(replace(replace(replace(c.homephone,'-',''),'(',''),')',''),' ',''),'') as homePhone, 
		isNull(replace(replace(replace(replace(e.workphone,'-',''),'(',''),')',''),' ',''),'') as workPhone,
		upper(isNull(c.emailaddress,'')) as email,
		case when c.dateofbirth is null then ''
		else convert(varchar(10),c.dateofbirth,121)
		end as birthday,
		ca.id as applicationID,
		c.ssn as SSN ,
		isNull(c.DriverLicenseNumber,'') as driverLicenseNumber,
		isNull(st2.StateAbbreviation,'') as issueState,
		isNull(case when isNull(cast(c.DriverlicenseExpirationDate as varchar(10)),'')='1900-01-01' then '' else cast(c.DriverlicenseExpirationDate as varchar(10)) end,'')  as  driverlicenseExpirationDate,
		upper(isNull(e.name,'')) as employerName,
		isNull(replace(replace(replace(e.workphone,'-',''),'(',''),')',''),'') as employerPhone,
		upper(isNull(e.Position,'')) as jobTitle,
		'TRUE' as isCurrentlyEmployed,
		s.netincome as monthlyIncomeNet,
		e.totalmonths as employmentMonthCount,
		case pc.paymenttypeid when '1' then '52' when '2' then '26' when '3' then '24' else '12' end as paymentsPerYear,
		convert(varchar(10),ca.datecreated,121) as dateOriginated,
		'60' as term,
		c.id as clientCustomerId,
		ca.id as clientLoanReferenceId,
		a.id as clientApplicationId,
		s.maxpurchaseprice as maxPurchasePrice, 
		case 
			when isNull(s.AdjustedMonthlyPayment,0)>0 then s.AdjustedMonthlyPayment 
			else s.MaxMonthlyPayment 
		end as maxMonthlyPayment,
		s.MinDownPaymentPercent AS minDownPaymentPercent,
		s.maxpurchaseprice as principalAmount,
		s.irate as annualInterestRate,
		upper(lot.LotName) as lotName,
		lot.streetAddress,
		lot.city,
		lot.postalCode,
		lot.[state],
		dv.outputxml,
		ca.datemodified AS dateModified
 
	FROM
	 CreditScoreApplication ca with (nolock) 
	 inner join Application a with (nolock)  on ca.cobuyerapplicationid=a.id
	 inner join Score s with (nolock)  on ca.primaryscoreid=s.id
	 inner join Customer c with (nolock)  on a.customerid=c.id
	 inner join Address ad  with (nolock) on a.addressid=ad.id
	 inner join employment e with (nolock)  on a.employmentid=e.id
	 inner join payingcapacity pc with (nolock)  on a.payingcapacityid=pc.id
	 inner join [state] st with (nolock)  on ad.stateid=st.id
	 left join  [state] st1 with (nolock)  on ad.prevstateid=st1.id
	 left join [state] st2 with (nolock)  on c.DriverLicenseStateID=st2.id
	 inner join streettype stt with (nolock)  on ad.streettypeid=stt.id
	 inner join housingtype ht with (nolock)  on pc.housingtypeid=ht.id
	 inner join lot lot with (nolock)  on ca.lotid=lot.id
	 inner join DataviewModelScore dv with (nolock)  on a.Dataviewmodelscoreid=dv.id
 
	WHERE 
	 ca.id = @CreditScoreApplicationID
	 ) b;


END
GO
/****** Object:  StoredProcedure [trustScience].[GetAppsToConvertToReports]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =========================================================================================================================
-- Author:		Sam Aloni
-- Create date: 10/20/2020
-- Description:	Collect list of applications that needed to call to get the report information
-- EXEC [trustScience].[GetAppsToConvertToReports];
-- =========================================================================================================================
CREATE PROCEDURE [trustScience].[GetAppsToConvertToReports]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- find the list date we processed
	SELECT [ID], [RequestID] 
	FROM 
		[dbo].[TrustScienceScore]  ts
	WHERE (([RetryCount] IS NULL AND [RecivedReportOn] IS NULL) OR		-- new reoprt
		  ([CallStatus] = 'Bad Request' OR QualifierCode1 = 'D14')) AND	-- get report failed before
		  (retryCount IS NULL OR RetryCount < 11)							-- do not try more than 10 time for now
	ORDER BY ID DESC;  -- get the older one first

END
GO
/****** Object:  StoredProcedure [trustScience].[SaveCreateFullScoringReqResp]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =================================================================================================
-- Author:		Sam Aloni
-- Create date: 11-20-2020
-- Description:	Save call request/response to Trust Science Create Full Scoring Request
-- =================================================================================================
CREATE PROCEDURE [trustScience].[SaveCreateFullScoringReqResp]
		@CustomerID int,
		@ApplicationID int,
		@Request varchar(max),
		@Response varchar(max),
		@CallStatus varchar(100),
		@RequestID varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[TrustScienceScore]
		 (
			    [CustomerID]
			   ,[ApplicationID]
			   ,[Request]
			   ,[CreateFullScoringResp]
			   ,[CreateDate]
			   ,[CallStatus]
			   ,[RequestID]
		 )
		 OUTPUT Inserted.Id
	VALUES
		 (
		 		@CustomerID
			   ,@ApplicationID
			   ,@Request
			   ,@Response
			   ,GETDATE()
			   ,@CallStatus
			   ,@RequestID
		 )

		
END
GO
/****** Object:  StoredProcedure [trustScience].[SaveGetReportRespone]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =================================================================================================
-- Author:		Sam Aloni
-- Create date: 11-20-2020
-- Description:	Save get report Response to Trust Science log Create [dbo].[TrustScienceScore]
-- =================================================================================================
CREATE PROCEDURE [trustScience].[SaveGetReportRespone]
		@LogID int,
		@RequestID varchar(100),
		@Score int,
		@QualifierCode1 varchar(20),
		@QualifierCodeDescription1 varchar(100),
		@QualifierCode2 varchar(20),
		@QualifierCodeDescription2 varchar(100),
		@QualifierCode3 varchar(20),
		@QualifierCodeDescription3 varchar(100),
		@QualifierCode4 varchar(20),
		@QualifierCodeDescription4 varchar(100),

		@ScoreReasonCode1 varchar(20),
		@ScoreReasonDescription1 varchar(100),
		@ScoreReasonCode2 varchar(20),
		@ScoreReasonDescription2 varchar(100),
		@ScoreReasonCode3 varchar(20),
		@ScoreReasonDescription3 varchar(100),
		@ScoreReasonCode4 varchar(20),
		@ScoreReasonDescription4 varchar(100),

		@ScoringDetailsURL varchar(250),
		@Response varchar(max),
		@CallStatus varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[TrustScienceScore]
   SET 
       [Score] = @Score
      ,[RequestID] = @RequestID
      ,[QualifierCode1] = @QualifierCode1
      ,[QualifierCodeDescription1] = @QualifierCodeDescription1
      ,[QualifierCode2] = @QualifierCode2
      ,[QualifierCodeDescription2] = @QualifierCodeDescription2
      ,[QualifierCode3] = @QualifierCode3
      ,[QualifierCodeDescription3] = @QualifierCodeDescription3
      ,[QualifierCode4] = @QualifierCode4
      ,[QualifierCodeDescription4] = @QualifierCodeDescription4

	  ,[ScoreReasonCode1] = @ScoreReasonCode1
	  ,[ScoreReasonDescription1] = @ScoreReasonDescription1
	  ,[ScoreReasonCode2] = @ScoreReasonCode2
	  ,[ScoreReasonDescription2] = @ScoreReasonDescription2
	  ,[ScoreReasonCode3] = @ScoreReasonCode3
	  ,[ScoreReasonDescription3] = @ScoreReasonDescription3
	  ,[ScoreReasonCode4] = @ScoreReasonCode4
	  ,[ScoreReasonDescription4] = @ScoreReasonDescription4

	  ,[RetryCount] = CASE 
						WHEN [RetryCount] IS NULL THEN 1
						ELSE [RetryCount] + 1
					  END

      ,[ScoringDetailsURL] = @ScoringDetailsURL
      ,[Response] = @Response
      ,[CallStatus] = @CallStatus
	  ,[RecivedReportOn] = GETDATE()
	WHERE ID = @LogID;

    -- insert the ref to the item into Application table
	DECLARE @ApplicationID int = 0

	SELECT @ApplicationID = ApplicationID FROM TrustScienceScore WHERE [RequestID] = @RequestID;

	UPDATE
		[dbo].[Application]
	SET
		[TrustScienceScoreID] = @LogID
	WHERE
		[Id] = @ApplicationID;

	-- Save the result to the the flow step log table [logs].[ApplicationFlowStepResult] 
	DECLARE @ApplicationFlowStepResultID int = 0

	SELECT 
		@ApplicationFlowStepResultID = afsr.id
	FROM
		[logs].[ApplicationFlowStepResult] afsr
	INNER JOIN
		[logs].[ClientApplication] ca ON afsr.ClientApplicationId = ca.id
	INNER JOIN
		[dbo].[ApplicationFlowStep] afs ON afsr.ApplicationFlowStepId = afs.Id
	WHERE
		ca.ApplicationId = @ApplicationID AND
		afs.StepName = 'Trust Science';




END
GO
/****** Object:  StoredProcedure [trustScience].[SaveProcessingResult]    Script Date: 11/21/2020 8:50:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =====================================================================
-- Author:		Sam Aloni
-- Create date: 10/20/2020
-- Description:	Save last processing information to log table
-- EXEC [trustScience].[SaveProcessingResult] '2020-09-28', 100, 92, 8 , 7, '2020-09-29', '2020-09-30';
-- SELECT * FROM [TrustScienceScoreProcessingResult];
-- =====================================================================
CREATE PROCEDURE [trustScience].[SaveProcessingResult]
	@LastItemDateTime datetime,
	@TotalItemCount int ,
	@SuccessfulItemCount int ,
	@FailedItemCount int,
	@ProcessingErrorItemCount int,
	@StartDateTime datetime, 
	@EndDateTime datetime

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [TrustScienceScoreProcessingResult]
        (
			 [LastItemDateTime]
			,[TotalItemCount]
			,[SuccessfulItemCount]
			,[FailedItemCount]
			,[ProcessingErrorItemCount]
			,[StartDateTime]
			,[EndDateTime]
		)
     VALUES
		(
			@LastItemDateTime,
			@TotalItemCount,
			@SuccessfulItemCount,
			@FailedItemCount,
			@ProcessingErrorItemCount,
			@StartDateTime,
			@EndDateTime
		)

END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'=============================================
 Author:		Travis / Sam
 Create date: 08/01/2020
 Description:	Get application data for Point Predictive

  08/07/2020 - remove spaces from phone numbers
 EXEC PointPredictiveGetApplication 281473;  236720; 236622;  236720;  236627;   236720   248804
 EXEC PointPredictiveGetApplication 236627;
 collect credit with status = "PendingPaycall"
 select top 100 * from creditscoreapplication where CreditAppStatusID IN ( 8, 9) order by id desc;
 =============================================' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'PointPredictiveGetApplication'
GO
