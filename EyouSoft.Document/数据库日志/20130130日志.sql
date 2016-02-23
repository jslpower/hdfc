


--2012-01-30   日志    供应商列表增加附件列



GO
-- =============================================
-- Author:		<王磊>
-- Create date: <2012-12-27>
-- Description:	<供应商景点视图>
-- =============================================
ALTER view [dbo].[view_Spot]
as
SELECT 
Id,
CompanyId,
ProvinceId,
CityId,
SupplierType,
StorePrice,
WJPrice,
DJPrice,
ZKPrice,
(select ProvinceName from tbl_CompanyProvince where ID=tbl_CompanySupplier.ProvinceId) as ProvinceName,
(select CityName from tbl_CompanyCity where ID=tbl_CompanySupplier.CityId) as CityName,UnitName,Star,
		(select top 1 * from tbl_SupplierContact where tbl_SupplierContact.SupplierId=tbl_CompanySupplier.Id
	    for xml raw,root('Root')) as ContactInfo,
IsDelete,
IssueTime,
(case  when datediff(day,IssueTime,getdate())=0 then 1 else 0 end) as IsNew,
(SELECT [FileName],[FilePath] FROM  tbl_SupplierFile where SupplierId=tbl_CompanySupplier.Id for xml raw,root('Root')) as FileInfo
 FROM tbl_CompanySupplier inner join tbl_SupplierSpot
  on tbl_CompanySupplier.Id=tbl_SupplierSpot.SupplierId
GO



GO

-- =============================================
-- Author:		<王磊>
-- Create date: <2012-12-27>
-- Description:	<供应商餐饮视图>
-- =============================================
ALTER view [dbo].[view_Restaurant]
as
SELECT 
Id,
CompanyId,
ProvinceId,
CityId,
(select ProvinceName from tbl_CompanyProvince where ID=tbl_CompanySupplier.ProvinceId) as ProvinceName,
(select CityName from tbl_CompanyCity where ID=tbl_CompanySupplier.CityId) as CityName,UnitName,Cuisine,
	   (select top 1 * from tbl_SupplierContact where tbl_SupplierContact.SupplierId=tbl_CompanySupplier.Id
	    for xml raw,root('Root')) as ContactInfo,
IsDelete,
IssueTime,
(SELECT [FileName],[FilePath] FROM  tbl_SupplierFile where SupplierId=tbl_CompanySupplier.Id for xml raw,root('Root')) as FileInfo
FROM tbl_CompanySupplier inner join tbl_SupplierRestaurant 
on tbl_CompanySupplier.Id=tbl_SupplierRestaurant.SupplierId
GO


GO

-- =============================================
-- Author:		<王磊>
-- Create date: <2012-12-28>
-- Description:	<供应商视图>
-- =============================================
ALTER view [dbo].[view_Supplier]
as
SELECT 
Id,
CompanyId,
ProvinceId,
CityId,
SupplierType,
(select ProvinceName from tbl_CompanyProvince where ID=tbl_CompanySupplier.ProvinceId) as ProvinceName,
(select CityName from tbl_CompanyCity where ID=tbl_CompanySupplier.CityId) as CityName,UnitName,
		(select top 1 * from tbl_SupplierContact where tbl_SupplierContact.SupplierId=tbl_CompanySupplier.Id 
	    for xml raw,root('Root')) as ContactInfo,
IsDelete,
IssueTime,
(SELECT [FileName],[FilePath] FROM  tbl_SupplierFile where SupplierId=tbl_CompanySupplier.Id for xml raw,root('Root')) as FileInfo
FROM tbl_CompanySupplier
GO


GO

-- =============================================
-- Author:		<王磊>
-- Create date: <2012-12-27>
-- Description:	<供应商酒店视图>
-- =============================================
ALTER view [dbo].[view_Hotel]
as
SELECT 
Id,
CompanyId,
ProvinceId,
CityId,
SupplierType,
(select ProvinceName from tbl_CompanyProvince where ID=tbl_CompanySupplier.ProvinceId) as ProvinceName,
(select CityName from tbl_CompanyCity where ID=tbl_CompanySupplier.CityId) as CityName,UnitName,Star,
		(select top 1 * from tbl_SupplierContact where tbl_SupplierContact.SupplierId=tbl_CompanySupplier.Id
	     for xml raw,root('Root')) as ContactInfo,
IsDelete,
IssueTime,
(SELECT [FileName],[FilePath] FROM  tbl_SupplierFile where SupplierId=tbl_CompanySupplier.Id for xml raw,root('Root')) as FileInfo
 FROM tbl_CompanySupplier inner join tbl_SupplierHotel
  on tbl_CompanySupplier.Id=tbl_SupplierHotel.SupplierId
GO






--  2012-01-31    以上日志服务器已经更新



-- =============================================
-- Author:		<王磊>
-- Create date: <2013-1-30>
-- Description:	<强行删除确认件>
-- Result :1:成功 0：失败						
-- =============================================
Create proc proc_Tour_Delete_Power
@TourId char(36),
@Result int output
as
begin
	declare @error int
	set @error=0

	begin transaction
	--5、出纳登账相关信息表（companyid 收支登记表Id 是否程序自动添加为1）
	delete from tbl_FinChuNaDengZhang where DengJiId in
	(select Id from tbl_FinCope 
	where CollectionId in (select PlanId from tbl_PlanDiJie where TourId=@TourId))--出纳地接收支信息
	set @error=@error+@@error
	
	delete from tbl_FinChuNaDengZhang where DengJiId in
	(select Id from tbl_FinCope 
	where CollectionId in (select PlanId from tbl_PlanPiao where TourId=@TourId))--出纳机票收支信息
	set @error=@error+@@error
	
	
	delete from tbl_FinChuNaDengZhang where DengJiId in
	(select Id from tbl_FinCope where CollectionId=@TourId)--出纳确认件收支信息
	set @error=@error+@@error


	
	--4、收支登记相关信息表（companyid ItemType ItemId收款为TourId 付款为票务或者地接Id）
	Delete from tbl_FinCope where CollectionId in  
	(select PlanId from tbl_PlanDiJie where TourId=@TourId)	--删除地接收支信息
	set @error=@error+@@error

	Delete from tbl_FinCope where CollectionId in  
	(select PlanId from tbl_PlanPiao where TourId=@TourId)	--删除地接收款信息
	set @error=@error+@@error

	Delete from tbl_FinCope where CollectionId=@TourId --删除确认件收款信息
	set @error=@error+@@error
	
	
	--1、确认件登记相关信息表
	Update tbl_Tour set IsDelete='1' where TourId=@TourId
	set @error=@error+@@error

	delete from tbl_TourReturnVisit where TourId=@TourId --团队回访
	set @error=@error+@@error

	delete from tbl_TourFile where TourId=@TourId
	set @error=@error+@@error

	delete from tbl_TourDiJie where TourId=@TourId
	set @error=@error+@@error

	delete from tbl_TourGuide where TourId=@TourId
	set @error=@error+@@error


	--2、地接安排相关信息表
	delete from tbl_PlanDiJieFile 
	where PlanId=(select PlanId from tbl_PlanDiJie where TourId=@TourId) --删除地接附件
	set @error=@error+@@error	 

	Delete from tbl_PlanDiJie where TourId=@TourId
	set @error=@error+@@error	

	
	
	--3、票务安排相关信息表
	Delete from tbl_PlanPiaoTraveller where TourId=@TourId --删掉游客信息
	set @error=@error+@@error	

	Delete from tbl_PlanPiaoFile where PlanId=(select PlanId from tbl_PlanPiao where TourId=@TourId) --附件信息
	set @error=@error+@@error	
	
	Delete from tbl_PlanPiao where TourId=@TourId	
	set @error=@error+@@error	

	if(@error=0)
	begin
		set @Result=1
		commit transaction
	end
	else
	begin
		set @Result=0
		rollback transaction
	end

	return @Result

end
Go


-- =============================================
-- Author:		<王磊>
-- Create date: <2012-11-20>
-- Description:	<删除确认件>
-- Result :-1:做过地接安排不允许删除
--		   -2:做过出票或退票安排不允许删除
--		   -3:财务审核后不能删除
--			-4:操作结束不能删除
--			1:成功 0：失败						
-- =============================================
ALTER proc [dbo].[proc_Tour_Delete]
@TourId char(36),
@Result int output
as
begin
	if exists(select 1 from tbl_Tour where IsEnd='1' and TourId=@TourId)
	begin
		set @Result=-4	--操作结束不能删除
		return @Result
	end

	if exists(select 1 from tbl_PlanDiJie where TourId=@TourId)
	begin
		set @Result=-1	--做过地接安排不允许删除
		return @Result
	end

	if exists(select 1 from tbl_PlanPiao where TourId=@TourId)
	begin
		set @Result=-2	--做过出票或退票安排不允许删除
		return @Result
	end

	if exists(select 1 from tbl_Tour where TourId=@TourId and TourStatus=1)
	begin
		set @Result=-3	--财务审核后不能删除
		return @Result
	end

	declare @error int
	set @error=0

	Update tbl_Tour set IsDelete='1' where TourId=@TourId
	set @error=@error+@@error
	
	if(@error=0)
	begin
		set @Result=1
	end
	else
	begin
		set @Result=0
	end

	return @Result

end
GO

---------------------------------------------------------------------------------------------


alter table tbl_Tour add Remark nvarchar(max)
GO



-- =============================================
-- Author:		<王磊>
-- Create date: <2012-12-21>
-- Description:	<确认件登记>
-- Result :0:添加失败 1:添加成功
-- =============================================
ALTER proc [dbo].[proc_Tour_Add]
@TourId char(36), 
@CompanyId int, 
@TourType tinyint, 
@RouteId char(36), 
@RouteName nvarchar(200), 
@IsRouteHuiTian char(1),
@LDate datetime, 
@RDate datetime, 
@SaleId int, 
@IsMonth char(1), 
@MonthTime nvarchar(50), 
@Adults int, 
@Childs int, 
@Accompanys int, 
@BuyCompanyId char(36), 
@SumPrice money, 
@OperatorId int, 
@TourStatus tinyint, 
@IsChuPiao char(1), 
@Remark nvarchar(max),
@TourDiJie xml,--地接信息
@TourGuide xml,--导游信息
@File xml,	   --附件
@Result int output
as
begin
	declare @error int
	set @error=0

	begin transaction
	--回填线路信息
	IF(@IsRouteHuiTian='1')
	begin
		if exists(select 1 from tbl_Route where RouteName=@RouteName and CompanyId=@CompanyId)
		begin
			select @RouteId=Id from tbl_Route where RouteName=@RouteName and CompanyId=@CompanyId
		end
		else
		begin
			set @RouteId=newId()
			INSERT INTO tbl_Route(Id,CompanyId,RouteName)VALUES(@RouteId,@CompanyId,@RouteName)
			set @error=@error+@@error
		end
		
	end
	
	INSERT INTO tbl_Tour
           (TourId,CompanyId,TourCode,TourType
           ,RouteId,RouteName,LDate,RDate
           ,SaleId,IsMonth,MonthTime,Adults
           ,Childs,Accompanys,BuyCompanyId,SumPrice
           ,OperatorId,TourStatus,IsChuPiao,Remark)
     VALUES(@TourId, @CompanyId,dbo.fn_TourCode(@CompanyId,@TourType,@LDate),@TourType,
           @RouteId, @RouteName,@LDate, @RDate,
           @SaleId, @IsMonth,@MonthTime, @Adults, 
           @Childs, @Accompanys,@BuyCompanyId, @SumPrice,
           @OperatorId, @TourStatus, @IsChuPiao,@Remark)
	set @error=@error+@@error

	if(@TourGuide is not null)
	begin
		declare @dydoc int
		exec sp_xml_preparedocument @dydoc output,@TourGuide

		INSERT INTO tbl_TourGuide(Id,TourId,GuideId,Phone)
		select Id,@TourId,GuideId,Phone
		from openxml(@dydoc,'/Root/TourGuide')
		with(Id char(36),GuideId char(36),Phone nvarchar(20))
		set @error=@error+@@error
		
		exec sp_xml_removedocument @dydoc
	end

	if(@File is not null)
	begin
		declare @fdoc int
		exec sp_xml_preparedocument @fdoc output,@File

		INSERT INTO tbl_TourFile(Id,TourId,[FileName],FilePath)
		select Id,@TourId,[FileName],FilePath
		from openxml(@fdoc,'/Root/File')
		with(Id char(36),[FileName] nvarchar(255),FilePath nvarchar(255))
		set @error=@error+@@error
		exec sp_xml_removedocument @fdoc
	end

	if(@error=0)
	begin
		set @Result=1
		commit transaction
		--rollback transaction
	end
	else
	begin
		set @Result=0
		rollback transaction
	end
	return @Result
end
GO



-- =============================================
-- Author:		<王磊>
-- Create date: <2012-12-21>
-- Description:	<修改确认件登记>
-- Result :-1:确认登记件财务已操作结束 无法修改
--			1:成功 0：失败
-- =============================================
ALTER proc [dbo].[proc_Tour_Update]
@TourId char(36), 
@CompanyId int, 
@RouteId char(36), 
@RouteName nvarchar(200), 
@IsRouteHuiTian char(1),
@LDate datetime, 
@RDate datetime, 
@SaleId int, 
@IsMonth char(1), 
@MonthTime datetime, 
@Adults int, 
@Childs int, 
@Accompanys int, 
@BuyCompanyId char(36), 
@SumPrice money, 
@IsChuPiao char(1), 
@Remark nvarchar(max),
@TourDiJie xml,--地接信息
@TourGuide xml,--导游信息
@File xml,	   --附件
@Result int output
as
begin
	
	if exists(select  1 from tbl_Tour where TourId=@TourId and IsEnd ='1')
	begin
		set @Result=-1	--确认登记件财务已操作结束 无法修改
		return @Result
	end
	
	declare @error int
	set @error=0
	
	begin transaction
		--回填线路信息
	IF(@IsRouteHuiTian='1')
	begin
		if exists(select 1 from tbl_Route where RouteName=@RouteName and CompanyId=@CompanyId)
		begin
			select @RouteId=Id from tbl_Route where RouteName=@RouteName and CompanyId=@CompanyId
		end
		else
		begin
			set @RouteId=newId()
			INSERT INTO tbl_Route(Id,CompanyId,RouteName)VALUES(@RouteId,@CompanyId,@RouteName)
			set @error=@error+@@error
		end
	end	

	UPDATE tbl_Tour SET RouteId = @RouteId,RouteName = @RouteName,
	LDate = @LDate,RDate = @RDate,SaleId = @SaleId, IsMonth = @IsMonth, 
	MonthTime = @MonthTime,Adults = @Adults, Childs = @Childs,
	Accompanys = @Accompanys,BuyCompanyId = @BuyCompanyId,
	SumPrice = @SumPrice,IsChuPiao = @IsChuPiao,Remark=@Remark 
	WHERE TourId=@TourId
	set @error=@error+@@error
	
	--删除导游信息

	
	delete from tbl_TourGuide where TourId=@TourId
	set @error=@error+@@error

	if(@TourGuide is not null)
	begin
		declare @dydoc int
		exec sp_xml_preparedocument @dydoc output,@TourGuide

		INSERT INTO tbl_TourGuide(Id,TourId,GuideId,Phone)
		select Id,@TourId,GuideId,Phone from openxml(@dydoc,'/Root/TourGuide')
		with(Id char(36),GuideId char(36),Phone nvarchar(20))
		set @error=@error+@@error
		
		exec sp_xml_removedocument @dydoc
	end
	
	--删除附件
	delete from tbl_TourFile where TourId=@TourId
	
	if(@File is not null)
	begin
		declare @fdoc int
		exec sp_xml_preparedocument @fdoc output,@File

		INSERT INTO tbl_TourFile(Id,TourId,[FileName],FilePath)
		select Id,@TourId,[FileName],FilePath
		from openxml(@fdoc,'/Root/File')
		with(Id char(36),[FileName] nvarchar(255),FilePath nvarchar(255))
		set @error=@error+@@error
		exec sp_xml_removedocument @fdoc
	end

	if(@error=0)
	begin
		set @Result=1
		commit transaction
	end	
	else
	begin
		set @Result=0
		rollback transaction
	end

	return @Result
end

GO


-- =============================================
-- Author:		<王磊>
-- Create date: <2012-12-21>
-- Description:	<财务保存操作>
-- Result :-1:只有未处理的订单才能修改
--			1:成功 0：失败
-- =============================================
ALTER proc [dbo].[proc_Tour_Fin_Update]
@TourId char(36), 
@CompanyId int, 
@RouteId char(36), 
@RouteName nvarchar(200), 
@IsRouteHuiTian char(1),
@LDate datetime, 
@RDate datetime, 
@SaleId int, 
@IsMonth char(1), 
@MonthTime datetime, 
@Adults int, 
@Childs int, 
@Accompanys int, 
@BuyCompanyId char(36), 
@SumPrice money, 
@IsChuPiao char(1), 
@Remark nvarchar(max),
@RebatePeople int,
@RebatePrice money,
@Profit money,
@ConfirmOperatorId int,--财务确认人
@ConfirmTime datetime,--财务确认时间	
@TourDiJie xml,--地接信息
@TourGuide xml,--导游信息
@File xml,	   --附件
@Result int output
as
begin
	
--	if not exists(select  1 from tbl_Tour where TourId=@TourId and TourStatus = 0)
--	begin
--		set @Result=-1	--财务已确认
--		return @Result
--	end

	
	
	declare @error int
	set @error=0
	
	begin transaction
		--回填线路信息
	IF(@IsRouteHuiTian='1')
	begin
		if exists(select 1 from tbl_Route where RouteName=@RouteName and CompanyId=@CompanyId)
		begin
			select @RouteId=Id from tbl_Route where RouteName=@RouteName and CompanyId=@CompanyId
		end
		else
		begin
			set @RouteId=newId()
			INSERT INTO tbl_Route(Id,CompanyId,RouteName)VALUES(@RouteId,@CompanyId,@RouteName)
			set @error=@error+@@error
		end
	end	

	declare @TourStatus tinyint
	select @TourStatus=TourStatus from tbl_Tour where TourId=@TourId
	if(@TourStatus=0)
	begin
		set @TourStatus=1
	end

	--财务保存操作将计划状态变为 未出发状态
	UPDATE tbl_Tour SET RouteId = @RouteId,RouteName = @RouteName,
	LDate = @LDate,RDate = @RDate,SaleId = @SaleId, IsMonth = @IsMonth, 
	MonthTime = @MonthTime,Adults = @Adults, Childs = @Childs,
	Accompanys = @Accompanys,BuyCompanyId = @BuyCompanyId,
	SumPrice = @SumPrice,IsChuPiao = @IsChuPiao ,Remark=@Remark,
	RebatePeople=@RebatePeople,RebatePrice=@RebatePrice,Profit=@Profit,
	TourStatus=@TourStatus,ConfirmOperatorId=@ConfirmOperatorId,ConfirmTime=@ConfirmTime
	WHERE TourId=@TourId
	set @error=@error+@@error
	
	--删除导游信息
	delete from tbl_TourGuide where TourId=@TourId
	set @error=@error+@@error

	if(@TourGuide is not null)
	begin
		declare @dydoc int
		exec sp_xml_preparedocument @dydoc output,@TourGuide

		INSERT INTO tbl_TourGuide(Id,TourId,GuideId,Phone)
		select Id,@TourId,GuideId,Phone from openxml(@dydoc,'/Root/TourGuide')
		with(Id char(36),GuideId char(36),Phone nvarchar(20))
		set @error=@error+@@error
		
		exec sp_xml_removedocument @dydoc
	end
	
	--删除附件
	delete from tbl_TourFile where TourId=@TourId
	
	if(@File is not null)
	begin
		declare @fdoc int
		exec sp_xml_preparedocument @fdoc output,@File

		INSERT INTO tbl_TourFile(Id,TourId,[FileName],FilePath)
		select Id,@TourId,[FileName],FilePath
		from openxml(@fdoc,'/Root/File')
		with(Id char(36),[FileName] nvarchar(255),FilePath nvarchar(255))
		set @error=@error+@@error
		exec sp_xml_removedocument @fdoc
	end

	if(@error=0)
	begin
		set @Result=1
		commit transaction
	end	
	else
	begin
		set @Result=0
		rollback transaction
	end

	return @Result
end
Go



-- =============================================
-- Author:		<王磊>
-- Create date: <2012-12-24>
-- Description:	<确认件详细信息>
-- =============================================
ALTER view [dbo].[view_Tour]
as
SELECT 
A.TourId,
A.CompanyId,
A.TourCode,
A.TourType,
A.RouteId,
A.RouteName,
A.LDate,
A.RDate,
A.SaleId,
(select ContactName from tbl_CompanyUser where Id=A.SaleId) as SaleName,--销售员名称
A.IsMonth,
A.MonthTime,
A.Adults,
A.Childs,
A.Accompanys,
A.BuyCompanyId,
(select CustomerName from tbl_Customer where Id=A.BuyCompanyId) as BuyCompanyName,
A.SumPrice,
A.OperatorId,
A.TourStatus,
A.IsChuPiao,
A.IssueTime,
A.CheckMoney,
A.ReturnMoney,
A.ReceivedMoney,
A.RefundMoney,
A.RebatePeople,
A.RebatePrice,
A.Profit,
A.ConfirmOperatorId,
A.ConfirmTime,
A.IsEnd,
A.Remark,
(SELECT B.Id,B.DiJieId,
(select UnitName from tbl_CompanySupplier where Id=B.Id) as DiJieName,--地接社名称
B.[Name],B.Phone,B.QQ 
FROM tbl_TourDiJie as B where B.TourId=A.TourId for xml raw,root('Root')) as TourDiJie,
(SELECT C.Id,C.GuideId,C.Phone, 
(select GuideName from tbl_SupplierGuide where Id=C.GuideId) as GuideName--导游名称
FROM dbo.tbl_TourGuide as C where C.TourId=A.TourId for xml raw,root('Root'))as TourGuide,
(SELECT D.Id,D.[FileName],D.FilePath
FROM tbl_TourFile as D where D.TourId=A.TourId for xml raw,root('Root')) as TourFile
FROM tbl_Tour as A
GO




-- =============================================
-- Author:		<王磊>
-- Create date: <2013-1-30>
-- Description:	<强行删除确认件>
-- Result :1:成功 0：失败						
-- =============================================
ALTER proc [dbo].[proc_Tour_Delete_Power]
@TourId char(36),
@Result int output
as
begin
	declare @error int
	set @error=0

	begin transaction
	--5、出纳登账相关信息表（companyid 收支登记表Id 是否程序自动添加为1）
	delete from tbl_FinChuNaDengZhang where DengJiId in
	(select Id from tbl_FinCope 
	where CollectionId in (select PlanId from tbl_PlanDiJie where TourId=@TourId))--出纳地接收支信息
	set @error=@error+@@error
	
	delete from tbl_FinChuNaDengZhang where DengJiId in
	(select Id from tbl_FinCope 
	where CollectionId in (select PlanId from tbl_PlanPiao where TourId=@TourId))--出纳机票收支信息
	set @error=@error+@@error
	
	
	delete from tbl_FinChuNaDengZhang where DengJiId in
	(select Id from tbl_FinCope where CollectionId=@TourId)--出纳确认件收支信息
	set @error=@error+@@error


	
	--4、收支登记相关信息表（companyid ItemType ItemId收款为TourId 付款为票务或者地接Id）
	Delete from tbl_FinCope where CollectionId in  
	(select PlanId from tbl_PlanDiJie where TourId=@TourId)	--删除地接收支信息
	set @error=@error+@@error

	Delete from tbl_FinCope where CollectionId in  
	(select PlanId from tbl_PlanPiao where TourId=@TourId)	--删除地接收款信息
	set @error=@error+@@error

	Delete from tbl_FinCope where CollectionId=@TourId --删除确认件收款信息
	set @error=@error+@@error
	
	
	--1、确认件登记相关信息表
	Update tbl_Tour set IsDelete='1' where TourId=@TourId
	set @error=@error+@@error

	delete from tbl_TourReturnVisit where TourId=@TourId --团队回访
	set @error=@error+@@error

	delete from tbl_TourFile where TourId=@TourId
	set @error=@error+@@error

	delete from tbl_TourDiJie where TourId=@TourId
	set @error=@error+@@error

	delete from tbl_TourGuide where TourId=@TourId
	set @error=@error+@@error

	--2、地接安排相关信息表
	delete from tbl_PlanDiJieFile 
	where PlanId in (select PlanId from tbl_PlanDiJie where TourId=@TourId) --删除地接附件
	set @error=@error+@@error	 

	Delete from tbl_PlanDiJie where TourId=@TourId
	set @error=@error+@@error	

	
	
	--3、票务安排相关信息表
	Delete from tbl_PlanPiaoTraveller where TourId=@TourId --删掉游客信息
	set @error=@error+@@error	

	Delete from tbl_PlanPiaoFile 
	where PlanId in (select PlanId from tbl_PlanPiao where TourId=@TourId) --附件信息
	set @error=@error+@@error	
	
	Delete from tbl_PlanPiao where TourId=@TourId	
	set @error=@error+@@error	

	if(@error=0)
	begin
		set @Result=1
		commit transaction
	end
	else
	begin
		set @Result=0
		rollback transaction
	end

	return @Result

end
GO



-- =============================================
-- Author:		<王磊>
-- Create date: <2013-1-30>
-- Description:	<修改团队状态>
-- Result :-1:团队操作已结束
--			1:成功 0：失败						
-- =============================================
Create proc [dbo].[proc_Tour_Update_Status]
@TourId char(36),
@TourStatus tinyint,
@Result int output
as
begin
	if exists(select 1 from tbl_Tour where IsEnd='1' and TourId=@TourId)
	begin
		set @Result=-1	--团队操作已结束
		return @Result
	end

	declare @error int
	set @error=0

	update tbl_Tour set TourStatus=@TourStatus where TourId=@TourId
	set @error=@error+@@error
	
	if(@error=0)
	begin
		set @Result=1
	end
	else
	begin
		set @Result=0
	end

	return @Result
end

GO



SET IDENTITY_INSERT [tbl_SysPrivs3] ON

--计调中心 确认件登记   --强制删除确认件
INSERT [tbl_SysPrivs3] ([Id],[ParentId],[Name],[SortId],[IsEnable],[PrivsType]) VALUES ( 141,1,'强制删除确认件',0,'1',0)

SET IDENTITY_INSERT [tbl_SysPrivs3] OFF



-------------------2012-1-31--    以上日志服务器已更新
go
alter table tbl_PlanDiJie add IsMonth char(1) default('0')
go
alter table tbl_PlanDiJie add MonthTime datetime
go

alter table tbl_PlanPiao add IsMonth char(1) default('0')
go
alter table tbl_PlanPiao add MonthTime datetime
go


update tbl_PlanDiJie set IsMonth='0'
GO

Update tbl_PlanPiao set IsMonth='0'

Go





-- =============================================
-- Author:		<王磊>
-- Create date: <2012-12-24>
-- Description:	<添加地接安排>
-- Result :		1:成功 0：失败 -1:供应商不存在
-- =============================================
ALTER proc [dbo].[proc_Dijie_Add]
@PlanId char(36),
@CompanyId int,
@TourId nvarchar(50),
@GysId char(36),
@Hotel money,
@Dining money,
@Car money,
@Ticket money,
@Guide money,
@Traffic money,
@Head money,
@AddPrice money,
@GuideIncome money,
@GuidePay money,
@Other money,
@PayType tinyint,
@SumPrice money,
@Remark nvarchar(255),
@OperatorId int,
@DiJieStatus tinyint,
@IsMonth char(1),
@MonthTime datetime,
@File xml,
@Result int output
as
begin
	declare @error int
	set @error=0
	if not exists(select 1 from tbl_CompanySupplier where @CompanyId=CompanyId and Id=@GysId)
	begin
		set @Result=-1	--供应商不存在
		return @Result
	end

	begin transaction
	INSERT INTO tbl_PlanDiJie
           (PlanId,CompanyId,TourId,GysId,Hotel,Dining,Car,Ticket
           ,Guide,Traffic,Head,AddPrice,GuideIncome,GuidePay
           ,Other,PayType,SumPrice,Remark,OperatorId,DiJieStatus,IsMonth,MonthTime)
     VALUES
           (@PlanId ,@CompanyId,@TourId,@GysId,@Hotel,@Dining,@Car,@Ticket
           ,@Guide,@Traffic,@Head,@AddPrice,@GuideIncome,@GuidePay
           ,@Other,@PayType,@SumPrice,@Remark,@OperatorId,@DiJieStatus,@IsMonth,@MonthTime)
	set @error=@error+@@error

	if(@File is not null)
	begin
		declare @fdoc int
		exec sp_xml_preparedocument @fdoc output,@File

		INSERT INTO tbl_PlanDiJieFile(PlanId,[FileName],FilePath)
		select @PlanId,[FileName],FilePath
		from openxml(@fdoc,'/Root/File')
		with([FileName] nvarchar(255),FilePath nvarchar(255))
		set @error=@error+@@error

		exec sp_xml_removedocument @fdoc
	end

	if(@error=0)
	begin
		set @Result=1
		commit transaction
	end
	else
	begin
		set @Result=0
		rollback transaction
	end

	return @Result
end

GO

-- =============================================
-- Author:		<王磊>
-- Create date: <2012-12-24>
-- Description:	<地接列表>
-- =============================================
ALTER view [dbo].[view_DiJie]
as
SELECT A.PlanId
	  ,A.CompanyId
      ,A.GysId
	  ,(select UnitName from tbl_CompanySupplier where Id=A.GysId) as GysName--供应商名称
	  ,(select ContactName,ContactTel,QQ from tbl_SupplierContact where SupplierId=A.GysId for xml raw,root('Root')) as ContactInfo--计调信息
      ,A.Hotel
      ,A.Dining
      ,A.Car
      ,A.Ticket
      ,A.Guide
      ,A.Traffic
	  ,(select TicketType,Interval,TrafficTime,TrafficNumber from tbl_PlanPiao where TourId=A.TourId for xml raw,root('Root'))as PlanPiaoInfo --大交通信息
      ,A.Head
      ,A.AddPrice
      ,A.GuideIncome
      ,A.GuidePay
      ,A.Other
      ,A.SumPrice
      ,A.DiJieStatus
	  ,A.IssueTime
	  ,A.IsMonth
	  ,A.MonthTime
	  ,B.TourCode
	  ,B.RouteName
	  ,B.LDate
      ,B.Adults
      ,B.Childs
      ,B.Accompanys
	  ,B.TourStatus
	  ,B.IsEnd
  FROM tbl_PlanDiJie as A inner join tbl_Tour as B
  on A.TourId=B.TourId
GO




-- =============================================
-- Author:		<王磊>
-- Create date: <2012-12-24>
-- Description:	<添加地接安排>
-- Result :	-1:只有地接状态在申请中才能修改	
-- -2:供应商不存在
--1:成功 0：失败
-- =============================================
ALTER proc [dbo].[proc_Dijie_Update]
@PlanId char(36),
@CompanyId int,
@TourId nvarchar(50),
@GysId char(36),
@Hotel money,
@Dining money,
@Car money,
@Ticket money,
@Guide money,
@Traffic money,
@Head money,
@AddPrice money,
@GuideIncome money,
@GuidePay money,
@Other money,
@PayType tinyint,
@SumPrice money,
@Remark nvarchar(255),
@IsMonth char(1),
@MonthTime datetime,
@File xml,
@Result int output
as
begin
	declare @error int
	set @error=0
	
	declare @DiJieStatus tinyint
	select @DiJieStatus= DiJieStatus from tbl_PlanDiJie where PlanId=@PlanId
	if(@DiJieStatus<>0)
	begin
		set @Result=-1 --只有地接状态在申请中才能修改
		return @Result
	end

	if not exists(select 1 from tbl_CompanySupplier where @CompanyId=CompanyId and Id=@GysId)
	begin
		set @Result=-2	--供应商不存在
		return @Result
	end
	
	begin transaction

    UPDATE tbl_PlanDiJie
    SET TourId = @TourId,GysId = @GysId,Hotel = @Hotel,Dining = @Dining
      ,Car = @Car,Ticket = @Ticket,Guide = @Guide,Traffic = @Traffic
      ,Head = @Head,AddPrice = @AddPrice,GuideIncome = @GuideIncome
      ,GuidePay = @GuidePay,Other = @Other,PayType = @PayType
      ,SumPrice = @SumPrice,Remark = @Remark,IsMonth=@IsMonth,MonthTime=@MonthTime
    WHERE PlanId=@PlanId

	set @error=@error+@@error

	delete from tbl_PlanDiJieFile where PlanId=@PlanId
	set @error=@error+@@error

	if(@File is not null)
	begin
		declare @fdoc int
		exec sp_xml_preparedocument @fdoc output,@File
		
		INSERT INTO tbl_PlanDiJieFile(PlanId,[FileName],FilePath)
		select @PlanId,[FileName],FilePath
		from openxml(@fdoc,'/Root/File')
		with([FileName] nvarchar(255),FilePath nvarchar(255))
		set @error=@error+@@error

		exec sp_xml_removedocument @fdoc
	end

	if(@error=0)
	begin
		set @Result=1
		commit transaction
	end
	else
	begin
		set @Result=0
		rollback transaction
	end

	return @Result
end



GO


-- =============================================
-- Author:		<王磊>
-- Create date: <2012-12-24>
-- Description:	<添加票务安排>
-- Result :		1:成功 0：失败 -1：供应商不存在
-- =============================================
ALTER proc [dbo].[proc_PlanPiao_Add]
@PlanId char(36),
@CompanyId int,
@TourId char(36),
@TicketType tinyint,
@TicketMode tinyint,
@GysId char(36),
@TicketerId int,
@TrafficNumber nvarchar(100),
@TrafficTime datetime,
@Interval nvarchar(100),
@Adults int,
@Childs int,
@AdultPrice money,
@ChildPrice money,
@OtherPrice money,
@Brokerage money,
@TrafficSeat nvarchar(100),
@PayType tinyint,
@SumPrice money,
@OperatorId int,
@TicketStatus tinyint,
@IsMonth char(1),
@MonthTime datetime,
@Traveller xml,
@File xml,
@Result int output
as
begin
	declare @error int
	set @error=0

	if not exists(select 1 from tbl_CompanySupplier where @CompanyId=CompanyId and Id=@GysId)
	begin
		set @Result=-1	--供应商不存在
		return @Result
	end

	begin transaction 

	INSERT INTO tbl_PlanPiao
           (PlanId,CompanyId,TourId,TicketType,TicketMode
           ,GysId,TicketerId,TrafficNumber,TrafficTime,Interval
           ,Adults,Childs,AdultPrice,ChildPrice,OtherPrice
           ,Brokerage,TrafficSeat,PayType,SumPrice,OperatorId,TicketStatus,IsMonth,MonthTime)
     VALUES(@PlanId,@CompanyId,@TourId,@TicketType,@TicketMode,
           @GysId,@TicketerId,@TrafficNumber,@TrafficTime,@Interval,
           @Adults,@Childs,@AdultPrice,@ChildPrice,@OtherPrice,
           @Brokerage,@TrafficSeat,@PayType,@SumPrice,@OperatorId,@TicketStatus,@IsMonth,@MonthTime)
	set @error=@error+@@error

	if(@Traveller is not null)
	begin
		declare @tdoc int
		exec sp_xml_preparedocument @tdoc output,@Traveller	


		INSERT INTO tbl_PlanPiaoTraveller
				   (TravellerId,PlanId,TourId,CompanyId,TravellerName
				   ,TravellerType,CardType,CardNumber,Birthday,Gender,Contact)
		select TravellerId,@PlanId,@TourId,@CompanyId,TravellerName,TravellerType,CardType,CardNumber,Birthday,Gender,Contact
		from openxml(@tdoc,'/Root/Traveller')
		with(TravellerId char(36),TravellerName nvarchar(50), TravellerType tinyint, 
             CardType tinyint, CardNumber nvarchar(50),Birthday nvarchar(100), Gender tinyint, Contact nvarchar(255))
		set @error=@error+@@error

		exec sp_xml_removedocument @tdoc
	end
	
	if(@File is not null)
	begin
		declare @fdoc int
		exec sp_xml_preparedocument @fdoc output,@File

		INSERT INTO tbl_PlanPiaoFile(PlanId,[FileName],FilePath)
		select @PlanId,[FileName],FilePath
		from openxml(@fdoc,'/Root/File')
		with([FileName] nvarchar(255),FilePath nvarchar(255))
		set @error=@error+@@error

		exec sp_xml_removedocument @fdoc
	end

	if(@error=0)
	begin
		set @Result=1
		commit transaction
	end
	else
	begin
		set @Result=0
		rollback transaction
	end

	return @Result
	
end


GO




-- =============================================
-- Author:		<王磊>
-- Create date: <2012-12-24>
-- Description:	<修改票务安排>
-- Result :-1:只有出票、退票状态在申请中才能修改	
--			-2:供应商不存在	
--		   1:成功 0：失败
-- =============================================
ALTER proc [dbo].[proc_PlanPiao_Update]
@PlanId char(36),
@CompanyId int,
@TourId char(36),
@TicketType tinyint,
@TicketMode tinyint,
@GysId char(36),
@TicketerId int,
@TrafficNumber nvarchar(100),
@TrafficTime datetime,
@Interval nvarchar(100),
@Adults int,
@Childs int,
@AdultPrice money,
@ChildPrice money,
@OtherPrice money,
@Brokerage money,
@TrafficSeat nvarchar(100),
@PayType tinyint,
@SumPrice money,
@IsMonth char(1),
@MonthTime datetime,
@Traveller xml,
@File xml,
@Result int output
as
begin
	declare @error int
	set @error=0
	if exists(select 1 from tbl_PlanPiao where PlanId = @PlanId and TicketStatus<>0)
	begin
		set @Result=-1	--只有出票、退票状态在申请中才能修改
		return @Result
	end
	
	if not exists(select 1 from tbl_CompanySupplier where @CompanyId=CompanyId and Id=@GysId)
	begin
		set @Result=-2	--供应商不存在
		return @Result
	end
	
	begin transaction
	UPDATE tbl_PlanPiao
	   SET TicketType = @TicketType, TicketMode = @TicketMode 
		  ,GysId = @GysId,TicketerId = @TicketerId,TrafficNumber = @TrafficNumber
		  ,TrafficTime = @TrafficTime,Interval = @Interval
		  ,Adults = @Adults,Childs = @Childs,AdultPrice = @AdultPrice
		  ,ChildPrice = @ChildPrice,OtherPrice = @OtherPrice
		  ,Brokerage = @Brokerage,TrafficSeat = @TrafficSeat
		  ,PayType = @PayType,SumPrice = @SumPrice,IsMonth=@IsMonth,MonthTime=@MonthTime
	 WHERE PlanId = @PlanId
	set @error=@error+@@error

	delete from tbl_PlanPiaoTraveller where PlanId=@PlanId
	set @error=@error+@@error


	if(@Traveller is not null)
	begin
		declare @tdoc int
		exec sp_xml_preparedocument @tdoc output,@Traveller	
		INSERT INTO tbl_PlanPiaoTraveller
				   (TravellerId,PlanId,TourId,CompanyId,TravellerName
				   ,TravellerType,CardType,CardNumber,Birthday,Gender,Contact)
		select TravellerId,@PlanId,@TourId,@CompanyId,TravellerName,TravellerType,CardType,CardNumber,Birthday,Gender,Contact
		from openxml(@tdoc,'/Root/Traveller')
		with(TravellerId char(36),TravellerName nvarchar(50), TravellerType tinyint, 
             CardType tinyint, CardNumber nvarchar(50),Birthday nvarchar(100), Gender tinyint, Contact nvarchar(255))
		set @error=@error+@@error

		exec sp_xml_removedocument @tdoc
	end

	delete from tbl_PlanPiaoFile where PlanId=@PlanId
	set @error=@error+@@error

	if(@File is not null)
	begin
		declare @fdoc int
		exec sp_xml_preparedocument @fdoc output,@File

		INSERT INTO tbl_PlanPiaoFile(PlanId,[FileName],FilePath)
		select @PlanId,[FileName],FilePath
		from openxml(@fdoc,'/Root/File')
		with([FileName] nvarchar(255),FilePath nvarchar(255))
		set @error=@error+@@error

		exec sp_xml_removedocument @fdoc
	end
	
	if(@error=0)
	begin
		set @Result=1
		commit transaction
	end
	else
	begin
		set @Result=0
		rollback transaction
	end

	return @Result
end

GO



--2013-02-25-----------------------------------------------------------------
GO
--=====================
--王磊 2013-02-25 批量更新客户等级
--=====================
declare @BuyCompanyId  char(36)
declare @count int
declare customer_cursor cursor for (select count(1),BuyCompanyId from tbl_Tour where  IsDelete='0' group by BuyCompanyId)
open customer_cursor
fetch next from customer_cursor into @count,@BuyCompanyId
while(@@fetch_status=0)
begin
--C级2
if(@count>=1 and @count<3)
begin
	update tbl_Customer set CustomerRating=2 where Id=@BuyCompanyId
end

--B级1
if(@count>=3 and @count<5)
begin
	update tbl_Customer set CustomerRating=1 where Id=@BuyCompanyId
end

--A级0
if(@count>=5)
begin
	update tbl_Customer set CustomerRating=0 where Id=@BuyCompanyId
end

fetch next from customer_cursor into @count,@BuyCompanyId

end

deallocate customer_cursor
Go

Go

-- =============================================
-- Author:		<王磊>
-- Create date: <2013-2-25>
-- Description:	<客户列表>
-- =============================================
create view  view_CustomerContact
as
select 
Id,
CompanyId,
ProviceId,
CityId,
SaleAreadId,
CustomerType,
CustomerName,
Licence,
Address,
PostalCode,
BankCode,
ContactName,
Phone,
Mobile,
Fax,
Remark,
OperatorId,
IssueTime,
CustomerRating ,
(select ProvinceName from tbl_CompanyProvince as a where a.Id = ProviceId) as ProvinceName  ,
(select CityName from tbl_CompanyCity as b where b.Id = CityId) as CityName ,
IsDelete
from 
tbl_Customer

union

select
A.Id,
A.CompanyId,
A.ProviceId,
A.CityId,
A.SaleAreadId,
A.CustomerType,
A.CustomerName,
A.Licence,
A.Address,
A.PostalCode,
A.BankCode,
B.Name as ContactName,
B.Tel as Phone,
B.Mobile,
B.Fax,
B.Remark,
A.OperatorId,
A.IssueTime,
A.CustomerRating ,
(select ProvinceName from tbl_CompanyProvince as p where P.Id = A.ProviceId) as ProvinceName  ,
(select CityName from tbl_CompanyCity as c where c.Id = A.CityId) as CityName ,
A.IsDelete
from tbl_Customer as A
left join tbl_CustomerContactInfo as B
 on A.Id=B.CustomerId

GO

GO

-- =============================================
-- Author:		<刘飞>
-- Create date: <2012-12-24>
-- Description:	<添加地接安排>
-- Result :	-1:只有地接状态在申请中和已确认才能修改	（已审核状态不能修改）
-- -2:供应商不存在
--1:成功 0：失败
-- =============================================
ALTER proc [dbo].[proc_Dijie_Update]
@PlanId char(36),
@CompanyId int,
@TourId nvarchar(50),
@GysId char(36),
@Hotel money,
@Dining money,
@Car money,
@Ticket money,
@Guide money,
@Traffic money,
@Head money,
@AddPrice money,
@GuideIncome money,
@GuidePay money,
@Other money,
@PayType tinyint,
@SumPrice money,
@Remark nvarchar(255),
@IsMonth char(1),
@MonthTime datetime,
@File xml,
@Result int output
as
begin
	declare @error int
	set @error=0
	
	declare @DiJieStatus tinyint
	select @DiJieStatus= DiJieStatus from tbl_PlanDiJie where PlanId=@PlanId
	if(@DiJieStatus=2)
	begin
		set @Result=-1 --只有地接状态在申请中和已确认才能修改（已审核状态不能修改）
		return @Result
	end

	if not exists(select 1 from tbl_CompanySupplier where @CompanyId=CompanyId and Id=@GysId)
	begin
		set @Result=-2	--供应商不存在
		return @Result
	end
	
	begin transaction

    UPDATE tbl_PlanDiJie
    SET TourId = @TourId,GysId = @GysId,Hotel = @Hotel,Dining = @Dining
      ,Car = @Car,Ticket = @Ticket,Guide = @Guide,Traffic = @Traffic
      ,Head = @Head,AddPrice = @AddPrice,GuideIncome = @GuideIncome
      ,GuidePay = @GuidePay,Other = @Other,PayType = @PayType
      ,SumPrice = @SumPrice,Remark = @Remark,IsMonth=@IsMonth,MonthTime=@MonthTime
    WHERE PlanId=@PlanId

	set @error=@error+@@error

	delete from tbl_PlanDiJieFile where PlanId=@PlanId
	set @error=@error+@@error

	if(@File is not null)
	begin
		declare @fdoc int
		exec sp_xml_preparedocument @fdoc output,@File
		
		INSERT INTO tbl_PlanDiJieFile(PlanId,[FileName],FilePath)
		select @PlanId,[FileName],FilePath
		from openxml(@fdoc,'/Root/File')
		with([FileName] nvarchar(255),FilePath nvarchar(255))
		set @error=@error+@@error

		exec sp_xml_removedocument @fdoc
	end

	if(@error=0)
	begin
		set @Result=1
		commit transaction
	end
	else
	begin
		set @Result=0
		rollback transaction
	end

	return @Result
end
GO

-- =============================================
-- Author:		<刘飞>
-- Create date: <2013-3-01>
-- Description:	<修改票务安排>
-- Result :-1:出票、退票状态审核后不能修改
--			-2:供应商不存在	
--		   1:成功 0：失败
-- =============================================
ALTER proc [dbo].[proc_PlanPiao_Update]
@PlanId char(36),
@CompanyId int,
@TourId char(36),
@TicketType tinyint,
@TicketMode tinyint,
@GysId char(36),
@TicketerId int,
@TrafficNumber nvarchar(100),
@TrafficTime datetime,
@Interval nvarchar(100),
@Adults int,
@Childs int,
@AdultPrice money,
@ChildPrice money,
@OtherPrice money,
@Brokerage money,
@TrafficSeat nvarchar(100),
@PayType tinyint,
@SumPrice money,
@IsMonth char(1),
@MonthTime datetime,
@Traveller xml,
@File xml,
@Result int output
as
begin
	declare @error int
	set @error=0
	if exists(select 1 from tbl_PlanPiao where PlanId = @PlanId and TicketStatus=2)
	begin
		set @Result=-1	--出票、退票状态审核后不能修改
		return @Result
	end
	
	if not exists(select 1 from tbl_CompanySupplier where @CompanyId=CompanyId and Id=@GysId)
	begin
		set @Result=-2	--供应商不存在
		return @Result
	end
	
	begin transaction
	UPDATE tbl_PlanPiao
	   SET TicketType = @TicketType, TicketMode = @TicketMode 
		  ,GysId = @GysId,TicketerId = @TicketerId,TrafficNumber = @TrafficNumber
		  ,TrafficTime = @TrafficTime,Interval = @Interval
		  ,Adults = @Adults,Childs = @Childs,AdultPrice = @AdultPrice
		  ,ChildPrice = @ChildPrice,OtherPrice = @OtherPrice
		  ,Brokerage = @Brokerage,TrafficSeat = @TrafficSeat
		  ,PayType = @PayType,SumPrice = @SumPrice,IsMonth=@IsMonth,MonthTime=@MonthTime
	 WHERE PlanId = @PlanId
	set @error=@error+@@error

	delete from tbl_PlanPiaoTraveller where PlanId=@PlanId
	set @error=@error+@@error


	if(@Traveller is not null)
	begin
		declare @tdoc int
		exec sp_xml_preparedocument @tdoc output,@Traveller	
		INSERT INTO tbl_PlanPiaoTraveller
				   (TravellerId,PlanId,TourId,CompanyId,TravellerName
				   ,TravellerType,CardType,CardNumber,Birthday,Gender,Contact)
		select TravellerId,@PlanId,@TourId,@CompanyId,TravellerName,TravellerType,CardType,CardNumber,Birthday,Gender,Contact
		from openxml(@tdoc,'/Root/Traveller')
		with(TravellerId char(36),TravellerName nvarchar(50), TravellerType tinyint, 
             CardType tinyint, CardNumber nvarchar(50),Birthday nvarchar(100), Gender tinyint, Contact nvarchar(255))
		set @error=@error+@@error

		exec sp_xml_removedocument @tdoc
	end

	delete from tbl_PlanPiaoFile where PlanId=@PlanId
	set @error=@error+@@error

	if(@File is not null)
	begin
		declare @fdoc int
		exec sp_xml_preparedocument @fdoc output,@File

		INSERT INTO tbl_PlanPiaoFile(PlanId,[FileName],FilePath)
		select @PlanId,[FileName],FilePath
		from openxml(@fdoc,'/Root/File')
		with([FileName] nvarchar(255),FilePath nvarchar(255))
		set @error=@error+@@error

		exec sp_xml_removedocument @fdoc
	end
	
	if(@error=0)
	begin
		set @Result=1
		commit transaction
	end
	else
	begin
		set @Result=0
		rollback transaction
	end

	return @Result
end
GO



--2013-3-25 修改记录
GO

-- =============================================
-- Author:		<王磊>
-- Create date: <2012-12-21>
-- Description:	<修改确认件登记>
-- Result :-1:确认登记件财务已操作结束 无法修改
--			1:成功 0：失败
--2013-3-25 团队类型可改变(对应的团号也改变)
-- =============================================
ALTER proc [dbo].[proc_Tour_Update]
@TourId char(36), 
@CompanyId int, 
@TourType tinyint,
@RouteId char(36), 
@RouteName nvarchar(200), 
@IsRouteHuiTian char(1),
@LDate datetime, 
@RDate datetime, 
@SaleId int, 
@IsMonth char(1), 
@MonthTime datetime, 
@Adults int, 
@Childs int, 
@Accompanys int, 
@BuyCompanyId char(36), 
@SumPrice money, 
@IsChuPiao char(1), 
@Remark nvarchar(max),
@TourDiJie xml,--地接信息
@TourGuide xml,--导游信息
@File xml,	   --附件
@Result int output
as
begin
	
	if exists(select  1 from tbl_Tour where TourId=@TourId and IsEnd ='1')
	begin
		set @Result=-1	--确认登记件财务已操作结束 无法修改
		return @Result
	end
	
	declare @error int
	set @error=0
	
	begin transaction
		--回填线路信息
	IF(@IsRouteHuiTian='1')
	begin
		if exists(select 1 from tbl_Route where RouteName=@RouteName and CompanyId=@CompanyId)
		begin
			select @RouteId=Id from tbl_Route where RouteName=@RouteName and CompanyId=@CompanyId
		end
		else
		begin
			set @RouteId=newId()
			INSERT INTO tbl_Route(Id,CompanyId,RouteName)VALUES(@RouteId,@CompanyId,@RouteName)
			set @error=@error+@@error
		end
	end	
	
	UPDATE tbl_Tour SET TourCode=dbo.fn_TourCode(@CompanyId,@TourType,@LDate),
	TourType=@TourType,RouteId = @RouteId,RouteName = @RouteName,
	LDate = @LDate,RDate = @RDate,SaleId = @SaleId, IsMonth = @IsMonth, 
	MonthTime = @MonthTime,Adults = @Adults, Childs = @Childs,
	Accompanys = @Accompanys,BuyCompanyId = @BuyCompanyId,
	SumPrice = @SumPrice,IsChuPiao = @IsChuPiao,Remark=@Remark 
	WHERE TourId=@TourId
	set @error=@error+@@error
	
	--删除导游信息

	
	delete from tbl_TourGuide where TourId=@TourId
	set @error=@error+@@error

	if(@TourGuide is not null)
	begin
		declare @dydoc int
		exec sp_xml_preparedocument @dydoc output,@TourGuide

		INSERT INTO tbl_TourGuide(Id,TourId,GuideId,Phone)
		select Id,@TourId,GuideId,Phone from openxml(@dydoc,'/Root/TourGuide')
		with(Id char(36),GuideId char(36),Phone nvarchar(20))
		set @error=@error+@@error
		
		exec sp_xml_removedocument @dydoc
	end
	
	--删除附件
	delete from tbl_TourFile where TourId=@TourId
	
	if(@File is not null)
	begin
		declare @fdoc int
		exec sp_xml_preparedocument @fdoc output,@File

		INSERT INTO tbl_TourFile(Id,TourId,[FileName],FilePath)
		select Id,@TourId,[FileName],FilePath
		from openxml(@fdoc,'/Root/File')
		with(Id char(36),[FileName] nvarchar(255),FilePath nvarchar(255))
		set @error=@error+@@error
		exec sp_xml_removedocument @fdoc
	end

	if(@error=0)
	begin
		set @Result=1
		commit transaction
	end	
	else
	begin
		set @Result=0
		rollback transaction
	end

	return @Result
end
Go







--  2013-03-26     --2013-3-28--------------------------------------



GO



-- =============================================
-- Author:		<周文超>
-- Create date: <2012-12-28>
-- Description:	<团散统计列表>
-- =============================================
ALTER view [dbo].[view_TourAndCustomer]
as

SELECT a.BuyCompanyId AS CustomerId,a.LDate,a.TourCode,a.Adults,a.Childs,a.Accompanys,a.RouteName,a.SumPrice,a.Profit,a.TourType,a.IssueTime,a.SaleId,a.OperatorId,a.TourId,a.CompanyId,a.IsDelete,a.ReceivedMoney,a.CheckMoney
,(ISNULL(a.RebatePeople,0) * ISNULL(a.RebatePrice,0)) AS YongJin
,b.CustomerName,b.ProviceId AS CustomerProviceId,b.CityId AS CustomerCityId,b.ContactName,b.Phone AS ContactTel,b.Mobile AS ContactMobile,b.IssueTime AS CustomerIssueTime,b.SaleAreadId
,c.ContactName AS OperatorName
,d.ContactName AS SaleName
,e.ProvinceName AS CustomerProvinceName
,f.CityName AS CustomerCityName
,g.SaleAreaName
,((SELECT ISNULL(SUM(pd.SumPrice),0) FROM tbl_PlanDiJie AS pd WHERE pd.TourId = a.TourId) + (SELECT ISNULL(SUM(pp.SumPrice),0) FROM tbl_PlanPiao AS pp WHERE pp.TourId = a.TourId)) AS ZongZhiChu
FROM 
tbl_Tour AS a LEFT JOIN tbl_Customer AS b ON a.BuyCompanyId = b.Id
LEFT JOIN tbl_CompanyUser AS c on a.OperatorId = c.Id
LEFT JOIN tbl_CompanyUser AS d ON a.SaleId = d.Id
LEFT JOIN tbl_CompanyProvince AS e on e.Id = b.ProviceId
LEFT JOIN tbl_CompanyCity AS f ON f.Id = b.CityId
LEFT JOIN tbl_CompanySaleArea AS g ON g.SaleAreaId = b.SaleAreadId
WHERE a.IsDelete = '0'


go

GO
CREATE TABLE [dbo].[SMS_CustomerCareforTour](
	[TourId] [char](36) NOT NULL CONSTRAINT [DF_SMS_CustomerCareforTour_TourId]  DEFAULT (''),
	[SmsId] [int] NOT NULL CONSTRAINT [DF_SMS_CustomerCareforTour_SmsId]  DEFAULT ((0)),
 CONSTRAINT [PK_SMS_CustomerCareforTour] PRIMARY KEY CLUSTERED 
(
	[TourId] ASC,
	[SmsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'团队编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMS_CustomerCareforTour', @level2type=N'COLUMN',@level2name=N'TourId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'短信关怀编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMS_CustomerCareforTour', @level2type=N'COLUMN',@level2name=N'SmsId'


go



GO
-- =============================================
-- Author:		<王磊>
-- Create date: <2012-12-21>
-- Description:	<生成控位号>
-- Result :系统根据出团日期自动生成控位号，出团日期-当天流水号（T/S）生成
-- =============================================
ALTER function [dbo].[fn_TourCode](
	@CompanyId int,		--公司号
	@TourType tinyint,	--团队类型
	@LDate datetime,	--出团时间
	@_Type tinyint		-- 0:add or 1:update
)
RETURNS varchar(200)
begin
	--Select CONVERT(varchar(100), @LDate, 112)
	declare @mark nvarchar(10)
	set @mark='T'
	if(@TourType=1)
	begin
		set @mark='S'
	end
	
	declare @code nvarchar(500)
	declare @index int
	if(@_Type=0)
	begin
		if not exists(select 1 from tbl_Tour  where CompanyId=@CompanyId and LDate=@LDate )
		begin
			select @index=count(1)+1 from tbl_Tour  where CompanyId=@CompanyId and LDate=@LDate 
		end
		else
		begin
			select @index=max(subString(TourCode,len(TourCode)-1,1))+1  from tbl_Tour where CompanyId=@CompanyId and LDate=@LDate
		end
	end
	if(@_Type=1)
	begin
		 select @index=max(subString(TourCode,len(TourCode)-1,1))+1  from tbl_Tour where CompanyId=@CompanyId and LDate=@LDate
	end

	if(@index>=1 and @index<=9)
	begin
		set @code= CONVERT(varchar(100), @LDate, 112)+'00'+cast(@index as nvarchar(10))+@mark
	end
	else if(@index>=10 and @index<=99)
	begin
		set @code= CONVERT(varchar(100), @LDate, 112)+'0'+cast(@index as nvarchar(10))+@mark
	end
	else
	begin
		set @code=CONVERT(varchar(100), @LDate, 112)+cast(@index as varchar(10))+@mark
	end
	
	return (@code)
end


GO



-- =============================================
-- Author:		<王磊>
-- Create date: <2012-12-21>
-- Description:	<确认件登记>
-- Result :0:添加失败 1:添加成功
-- =============================================
ALTER proc [dbo].[proc_Tour_Add]
@TourId char(36), 
@CompanyId int, 
@TourType tinyint, 
@RouteId char(36), 
@RouteName nvarchar(200), 
@IsRouteHuiTian char(1),
@LDate datetime, 
@RDate datetime, 
@SaleId int, 
@IsMonth char(1), 
@MonthTime nvarchar(50), 
@Adults int, 
@Childs int, 
@Accompanys int, 
@BuyCompanyId char(36), 
@SumPrice money, 
@OperatorId int, 
@TourStatus tinyint, 
@IsChuPiao char(1), 
@Remark nvarchar(max),
@TourDiJie xml,--地接信息
@TourGuide xml,--导游信息
@File xml,	   --附件
@Result int output
as
begin
	declare @error int
	set @error=0

	begin transaction
	--回填线路信息
	IF(@IsRouteHuiTian='1')
	begin
		if exists(select 1 from tbl_Route where RouteName=@RouteName and CompanyId=@CompanyId)
		begin
			select @RouteId=Id from tbl_Route where RouteName=@RouteName and CompanyId=@CompanyId
		end
		else
		begin
			set @RouteId=newId()
			INSERT INTO tbl_Route(Id,CompanyId,RouteName)VALUES(@RouteId,@CompanyId,@RouteName)
			set @error=@error+@@error
		end
		
	end
	
	INSERT INTO tbl_Tour
           (TourId,CompanyId,TourCode,TourType
           ,RouteId,RouteName,LDate,RDate
           ,SaleId,IsMonth,MonthTime,Adults
           ,Childs,Accompanys,BuyCompanyId,SumPrice
           ,OperatorId,TourStatus,IsChuPiao,Remark)
     VALUES(@TourId, @CompanyId,dbo.fn_TourCode(@CompanyId,@TourType,@LDate,0),@TourType,
           @RouteId, @RouteName,@LDate, @RDate,
           @SaleId, @IsMonth,@MonthTime, @Adults, 
           @Childs, @Accompanys,@BuyCompanyId, @SumPrice,
           @OperatorId, @TourStatus, @IsChuPiao,@Remark)
	set @error=@error+@@error

	if(@TourGuide is not null)
	begin
		declare @dydoc int
		exec sp_xml_preparedocument @dydoc output,@TourGuide

		INSERT INTO tbl_TourGuide(Id,TourId,GuideId,Phone)
		select Id,@TourId,GuideId,Phone
		from openxml(@dydoc,'/Root/TourGuide')
		with(Id char(36),GuideId char(36),Phone nvarchar(20))
		set @error=@error+@@error
		
		exec sp_xml_removedocument @dydoc
	end

	if(@File is not null)
	begin
		declare @fdoc int
		exec sp_xml_preparedocument @fdoc output,@File

		INSERT INTO tbl_TourFile(Id,TourId,[FileName],FilePath)
		select Id,@TourId,[FileName],FilePath
		from openxml(@fdoc,'/Root/File')
		with(Id char(36),[FileName] nvarchar(255),FilePath nvarchar(255))
		set @error=@error+@@error
		exec sp_xml_removedocument @fdoc
	end

	if(@error=0)
	begin
		set @Result=1
		commit transaction
		--rollback transaction
	end
	else
	begin
		set @Result=0
		rollback transaction
	end
	return @Result
end



GO


-- =============================================
-- Author:		<王磊>
-- Create date: <2012-12-21>
-- Description:	<修改确认件登记>
-- Result :-1:确认登记件财务已操作结束 无法修改
--			1:成功 0：失败
--2013-3-25 团队类型可改变(对应的团号也改变)
-- =============================================
ALTER proc [dbo].[proc_Tour_Update]
@TourId char(36), 
@CompanyId int, 
@TourType tinyint,
@RouteId char(36), 
@RouteName nvarchar(200), 
@IsRouteHuiTian char(1),
@LDate datetime, 
@RDate datetime, 
@SaleId int, 
@IsMonth char(1), 
@MonthTime datetime, 
@Adults int, 
@Childs int, 
@Accompanys int, 
@BuyCompanyId char(36), 
@SumPrice money, 
@IsChuPiao char(1), 
@Remark nvarchar(max),
@TourDiJie xml,--地接信息
@TourGuide xml,--导游信息
@File xml,	   --附件
@Result int output
as
begin
	
	if exists(select  1 from tbl_Tour where TourId=@TourId and IsEnd ='1')
	begin
		set @Result=-1	--确认登记件财务已操作结束 无法修改
		return @Result
	end
	
	declare @error int
	set @error=0
	
	begin transaction
		--回填线路信息
	IF(@IsRouteHuiTian='1')
	begin
		if exists(select 1 from tbl_Route where RouteName=@RouteName and CompanyId=@CompanyId)
		begin
			select @RouteId=Id from tbl_Route where RouteName=@RouteName and CompanyId=@CompanyId
		end
		else
		begin
			set @RouteId=newId()
			INSERT INTO tbl_Route(Id,CompanyId,RouteName)VALUES(@RouteId,@CompanyId,@RouteName)
			set @error=@error+@@error
		end
	end	

	UPDATE tbl_Tour SET TourCode=dbo.fn_TourCode(@CompanyId,@TourType,@LDate,1),
	TourType=@TourType,RouteId = @RouteId,RouteName = @RouteName,
	LDate = @LDate,RDate = @RDate,SaleId = @SaleId, IsMonth = @IsMonth, 
	MonthTime = @MonthTime,Adults = @Adults, Childs = @Childs,
	Accompanys = @Accompanys,BuyCompanyId = @BuyCompanyId,
	SumPrice = @SumPrice,IsChuPiao = @IsChuPiao,Remark=@Remark 
	WHERE TourId=@TourId
	set @error=@error+@@error
	
	--删除导游信息

	
	delete from tbl_TourGuide where TourId=@TourId
	set @error=@error+@@error

	if(@TourGuide is not null)
	begin
		declare @dydoc int
		exec sp_xml_preparedocument @dydoc output,@TourGuide

		INSERT INTO tbl_TourGuide(Id,TourId,GuideId,Phone)
		select Id,@TourId,GuideId,Phone from openxml(@dydoc,'/Root/TourGuide')
		with(Id char(36),GuideId char(36),Phone nvarchar(20))
		set @error=@error+@@error
		
		exec sp_xml_removedocument @dydoc
	end
	
	--删除附件
	delete from tbl_TourFile where TourId=@TourId
	
	if(@File is not null)
	begin
		declare @fdoc int
		exec sp_xml_preparedocument @fdoc output,@File

		INSERT INTO tbl_TourFile(Id,TourId,[FileName],FilePath)
		select Id,@TourId,[FileName],FilePath
		from openxml(@fdoc,'/Root/File')
		with(Id char(36),[FileName] nvarchar(255),FilePath nvarchar(255))
		set @error=@error+@@error
		exec sp_xml_removedocument @fdoc
	end

	if(@error=0)
	begin
		set @Result=1
		commit transaction
	end	
	else
	begin
		set @Result=0
		rollback transaction
	end

	return @Result
end




GO


go

SET IDENTITY_INSERT [tbl_SysPrivs3] ON

--计调中心 确认件登记 --取消确认件登记
INSERT [tbl_SysPrivs3] ([Id],[ParentId],[Name],[SortId],[IsEnable],[PrivsType]) VALUES ( 142,1,'取消确认件登记',0,'1',0)

SET IDENTITY_INSERT [tbl_SysPrivs3] OFF

go




--         2013-04-08                  --------------------------------------------


GO

-- =============================================
-- Author:		<王磊>
-- Create date: <2012-12-21>
-- Description:	<生成控位号>
-- Result :系统根据出团日期自动生成控位号，出团日期-当天流水号（T/S）生成
-- =============================================
ALTER function [dbo].[fn_TourCode](
	@CompanyId int,		--公司号
	@TourType tinyint,	--团队类型
	@LDate datetime,	--出团时间
	@_Type tinyint		-- 0:add or 1:update
)
RETURNS varchar(200)
begin
	--Select CONVERT(varchar(100), @LDate, 112)
	declare @mark nvarchar(10)
	set @mark='T'
	if(@TourType=1)
	begin
		set @mark='S'
	end
	
	declare @code nvarchar(500)
	declare @index int
	if(@_Type=0)
	begin
		if not exists(select 1 from tbl_Tour  where CompanyId=@CompanyId and LDate=@LDate )
		begin
			select @index=count(1)+1 from tbl_Tour  where CompanyId=@CompanyId and LDate=@LDate 
		end
		else
		begin
			select @index=max(subString(TourCode,len(TourCode)-3,3))+1  from tbl_Tour where CompanyId=@CompanyId and LDate=@LDate
		end
	end
	if(@_Type=1)
	begin
		 select @index=max(subString(TourCode,len(TourCode)-3,3))+1  from tbl_Tour where CompanyId=@CompanyId and LDate=@LDate
	end

	if(@index>=1 and @index<=9)
	begin
		set @code=substring(CONVERT(varchar(100), @LDate, 112),3,6)+'00'+cast(@index as nvarchar(10))+@mark
	end
	else if(@index>=10 and @index<=99)
	begin
		set @code=substring(CONVERT(varchar(100), @LDate, 112),3,6)+'0'+cast(@index as nvarchar(10))+@mark
	end
	else
	begin
		set @code=substring(CONVERT(varchar(100), @LDate, 112),3,6)+cast(@index as varchar(10))+@mark
	end
	
	return (@code)
end

GO
alter table tbl_SupplierGuide add GysName nvarchar(250)
GO
alter table tbl_SupplierGuide add GysId char(36)

Go


-- =============================================
-- Author:		<王磊>
-- Create date: <2012-12-27>
-- Description:	<添加导游>
-- Result :0:添加失败 1:添加成功
-- =============================================
ALTER proc [dbo].[proc_Supplier_Guide_Add]
@Id char(36),
@CompanyId int,
@GysId char(36),
@GysName nvarchar(250),
@ProvinceId int,
@CityId int,
@GuideName nvarchar(20),
@Phone nvarchar(20),
@Birthday datetime,
@TourTime nvarchar(50),
@Remark nvarchar(250),
@Star tinyint,
@Belongs nvarchar(100),
@OperatorId int,
@File xml,
@Result int output
as
begin
	declare @error int
	set @error=0

	begin transaction
	INSERT INTO tbl_SupplierGuide
           (Id,CompanyId,GysId,GysName,ProvinceId,CityId,GuideName
           ,Phone,Birthday,TourTime,Remark,Star,Belongs,OperatorId)
     VALUES(@Id,@CompanyId,@GysId,@GysName,@ProvinceId,@CityId,@GuideName,
           @Phone,@Birthday,@TourTime,@Remark,@Star,@Belongs,@OperatorId)
	set @error=@error+@@error

	if(@File is not null)
	begin
		declare @fdoc int
		exec sp_xml_preparedocument @fdoc output,@File
		
		INSERT INTO tbl_SupplierFile(SupplierId,[FileName],FilePath,FileMode)
		SELECT @Id,[FileName],FilePath,FileMode
		FROM OPENXML(@fdoc,'/Root/File')
		WITH([FileName] nvarchar(255),FilePath nvarchar(255),FileMode tinyint)
		set @error=@error+@@error
		
		exec sp_xml_removedocument @fdoc
	end

	if(@error=0)
	begin
		set @Result=1
		commit transaction
	end
	else
	begin
		set @Result=0
		rollback transaction
	end
	return @Result
	
end
Go





-- =============================================
-- Author:		<王磊>
-- Create date: <2012-12-27>
-- Description:	<修改导游>
-- Result :0:失败 1:成功
-- =============================================
ALTER proc [dbo].[proc_Supplier_Guide_Update]
@Id char(36),
@CompanyId int,
@GysId char(36),
@GysName nvarchar(250),
@ProvinceId int,
@CityId int,
@GuideName nvarchar(20),
@Phone nvarchar(20),
@Birthday datetime,
@TourTime nvarchar(50),
@Remark nvarchar(250),
@Star tinyint,
@Belongs nvarchar(100),
@File xml,
@Result int output
as
begin
	declare @error int
	set @error=0

	begin transaction
	UPDATE tbl_SupplierGuide
    SET ProvinceId = @ProvinceId,CityId = @CityId,GysId=@GysId,GysName=@GysName,GuideName = @GuideName,Phone = @Phone
      ,Birthday = @Birthday,TourTime = @TourTime,Remark = @Remark,Star = @Star,Belongs = @Belongs
    WHERE Id = @Id
	set @error=@error+@@error

	delete from tbl_SupplierFile where SupplierId=@Id
	set @error=@error+@@error

	if(@File is not null)
	begin
		declare @fdoc int
		exec sp_xml_preparedocument @fdoc output,@File
		
		INSERT INTO tbl_SupplierFile(SupplierId,[FileName],FilePath,FileMode)
		SELECT @Id,[FileName],FilePath,FileMode
		FROM OPENXML(@fdoc,'/Root/File')
		WITH([FileName] nvarchar(255),FilePath nvarchar(255),FileMode tinyint)
		set @error=@error+@@error
		
		exec sp_xml_removedocument @fdoc
	end

	if(@error=0)
	begin
		set @Result=1
		commit transaction
	end
	else
	begin
		set @Result=0
		rollback transaction
	end
	return @Result
	
end

GO


-- =============================================
-- Author:		<王磊>
-- Create date: <2012-12-21>
-- Description:	<修改确认件登记>
-- Result :-1:确认登记件财务已操作结束 无法修改
--			1:成功 0：失败
--2013-3-25 团队类型可改变(对应的团号也改变)
-- =============================================
ALTER proc [dbo].[proc_Tour_Update]
@TourId char(36), 
@CompanyId int, 
@TourType tinyint,
@RouteId char(36), 
@RouteName nvarchar(200), 
@IsRouteHuiTian char(1),
@LDate datetime, 
@RDate datetime, 
@SaleId int, 
@IsMonth char(1), 
@MonthTime datetime, 
@Adults int, 
@Childs int, 
@Accompanys int, 
@BuyCompanyId char(36), 
@SumPrice money, 
@IsChuPiao char(1), 
@Remark nvarchar(max),
@TourDiJie xml,--地接信息
@TourGuide xml,--导游信息
@File xml,	   --附件
@Result int output
as
begin
	
	if exists(select  1 from tbl_Tour where TourId=@TourId and IsEnd ='1')
	begin
		set @Result=-1	--确认登记件财务已操作结束 无法修改
		return @Result
	end
	
	declare @error int
	set @error=0
	
	begin transaction
		--回填线路信息
	IF(@IsRouteHuiTian='1')
	begin
		if exists(select 1 from tbl_Route where RouteName=@RouteName and CompanyId=@CompanyId)
		begin
			select @RouteId=Id from tbl_Route where RouteName=@RouteName and CompanyId=@CompanyId
		end
		else
		begin
			set @RouteId=newId()
			INSERT INTO tbl_Route(Id,CompanyId,RouteName)VALUES(@RouteId,@CompanyId,@RouteName)
			set @error=@error+@@error
		end
	end	
	
	--如果出团时间发生改变则修改团号 否则不修改
	if not exists(select 1 from tbl_Tour where TourId=@TourId and LDate=@LDate and TourType=@TourType)
	begin
		UPDATE tbl_Tour SET TourCode=dbo.fn_TourCode(@CompanyId,@TourType,@LDate,1)
		WHERE TourId=@TourId
		set @error=@error+@@error
	end



	UPDATE tbl_Tour SET TourType=@TourType,RouteId = @RouteId,RouteName = @RouteName,
	LDate = @LDate,RDate = @RDate,SaleId = @SaleId, IsMonth = @IsMonth, 
	MonthTime = @MonthTime,Adults = @Adults, Childs = @Childs,
	Accompanys = @Accompanys,BuyCompanyId = @BuyCompanyId,
	SumPrice = @SumPrice,IsChuPiao = @IsChuPiao,Remark=@Remark 
	WHERE TourId=@TourId
	set @error=@error+@@error
	
	--删除导游信息

	
	delete from tbl_TourGuide where TourId=@TourId
	set @error=@error+@@error

	if(@TourGuide is not null)
	begin
		declare @dydoc int
		exec sp_xml_preparedocument @dydoc output,@TourGuide

		INSERT INTO tbl_TourGuide(Id,TourId,GuideId,Phone)
		select Id,@TourId,GuideId,Phone from openxml(@dydoc,'/Root/TourGuide')
		with(Id char(36),GuideId char(36),Phone nvarchar(20))
		set @error=@error+@@error
		
		exec sp_xml_removedocument @dydoc
	end
	
	--删除附件
	delete from tbl_TourFile where TourId=@TourId
	
	if(@File is not null)
	begin
		declare @fdoc int
		exec sp_xml_preparedocument @fdoc output,@File

		INSERT INTO tbl_TourFile(Id,TourId,[FileName],FilePath)
		select Id,@TourId,[FileName],FilePath
		from openxml(@fdoc,'/Root/File')
		with(Id char(36),[FileName] nvarchar(255),FilePath nvarchar(255))
		set @error=@error+@@error
		exec sp_xml_removedocument @fdoc
	end

	if(@error=0)
	begin
		set @Result=1
		commit transaction
	end	
	else
	begin
		set @Result=0
		rollback transaction
	end

	return @Result
end
GO


go

alter table tbl_PlanPiao alter column TrafficTime nvarchar(255)

alter table tbl_CompanyTicket alter column TrafficTime nvarchar(255)

GO

ALTER proc [dbo].[proc_PlanPiao_Add]
@PlanId char(36),
@CompanyId int,
@TourId char(36),
@TicketType tinyint,
@TicketMode tinyint,
@GysId char(36),
@TicketerId int,
@TrafficNumber nvarchar(100),
@TrafficTime nvarchar(100),
@Interval nvarchar(100),
@Adults int,
@Childs int,
@AdultPrice money,
@ChildPrice money,
@OtherPrice money,
@Brokerage money,
@TrafficSeat nvarchar(100),
@PayType tinyint,
@SumPrice money,
@OperatorId int,
@TicketStatus tinyint,
@IsMonth char(1),
@MonthTime datetime,
@Traveller xml,
@File xml,
@Result int output
as
begin
	declare @error int
	set @error=0

	if not exists(select 1 from tbl_CompanySupplier where @CompanyId=CompanyId and Id=@GysId)
	begin
		set @Result=-1	--供应商不存在
		return @Result
	end

	begin transaction 

	INSERT INTO tbl_PlanPiao
           (PlanId,CompanyId,TourId,TicketType,TicketMode
           ,GysId,TicketerId,TrafficNumber,TrafficTime,Interval
           ,Adults,Childs,AdultPrice,ChildPrice,OtherPrice
           ,Brokerage,TrafficSeat,PayType,SumPrice,OperatorId,TicketStatus,IsMonth,MonthTime)
     VALUES(@PlanId,@CompanyId,@TourId,@TicketType,@TicketMode,
           @GysId,@TicketerId,@TrafficNumber,@TrafficTime,@Interval,
           @Adults,@Childs,@AdultPrice,@ChildPrice,@OtherPrice,
           @Brokerage,@TrafficSeat,@PayType,@SumPrice,@OperatorId,@TicketStatus,@IsMonth,@MonthTime)
	set @error=@error+@@error

	if(@Traveller is not null)
	begin
		declare @tdoc int
		exec sp_xml_preparedocument @tdoc output,@Traveller	


		INSERT INTO tbl_PlanPiaoTraveller
				   (TravellerId,PlanId,TourId,CompanyId,TravellerName
				   ,TravellerType,CardType,CardNumber,Birthday,Gender,Contact)
		select TravellerId,@PlanId,@TourId,@CompanyId,TravellerName,TravellerType,CardType,CardNumber,Birthday,Gender,Contact
		from openxml(@tdoc,'/Root/Traveller')
		with(TravellerId char(36),TravellerName nvarchar(50), TravellerType tinyint, 
             CardType tinyint, CardNumber nvarchar(50),Birthday nvarchar(100), Gender tinyint, Contact nvarchar(255))
		set @error=@error+@@error

		exec sp_xml_removedocument @tdoc
	end
	
	if(@File is not null)
	begin
		declare @fdoc int
		exec sp_xml_preparedocument @fdoc output,@File

		INSERT INTO tbl_PlanPiaoFile(PlanId,[FileName],FilePath)
		select @PlanId,[FileName],FilePath
		from openxml(@fdoc,'/Root/File')
		with([FileName] nvarchar(255),FilePath nvarchar(255))
		set @error=@error+@@error

		exec sp_xml_removedocument @fdoc
	end

	if(@error=0)
	begin
		set @Result=1
		commit transaction
	end
	else
	begin
		set @Result=0
		rollback transaction
	end

	return @Result
	
end
GO

ALTER proc [dbo].[proc_PlanPiao_Update]
@PlanId char(36),
@CompanyId int,
@TourId char(36),
@TicketType tinyint,
@TicketMode tinyint,
@GysId char(36),
@TicketerId int,
@TrafficNumber nvarchar(100),
@TrafficTime nvarchar(100),
@Interval nvarchar(100),
@Adults int,
@Childs int,
@AdultPrice money,
@ChildPrice money,
@OtherPrice money,
@Brokerage money,
@TrafficSeat nvarchar(100),
@PayType tinyint,
@SumPrice money,
@IsMonth char(1),
@MonthTime datetime,
@Traveller xml,
@File xml,
@Result int output
as
begin
	declare @error int
	set @error=0
	if exists(select 1 from tbl_PlanPiao where PlanId = @PlanId and TicketStatus=2)
	begin
		set @Result=-1	--出票、退票状态审核后不能修改
		return @Result
	end
	
	if not exists(select 1 from tbl_CompanySupplier where @CompanyId=CompanyId and Id=@GysId)
	begin
		set @Result=-2	--供应商不存在
		return @Result
	end
	
	begin transaction
	UPDATE tbl_PlanPiao
	   SET TicketType = @TicketType, TicketMode = @TicketMode 
		  ,GysId = @GysId,TicketerId = @TicketerId,TrafficNumber = @TrafficNumber
		  ,TrafficTime = @TrafficTime,Interval = @Interval
		  ,Adults = @Adults,Childs = @Childs,AdultPrice = @AdultPrice
		  ,ChildPrice = @ChildPrice,OtherPrice = @OtherPrice
		  ,Brokerage = @Brokerage,TrafficSeat = @TrafficSeat
		  ,PayType = @PayType,SumPrice = @SumPrice,IsMonth=@IsMonth,MonthTime=@MonthTime
	 WHERE PlanId = @PlanId
	set @error=@error+@@error

	delete from tbl_PlanPiaoTraveller where PlanId=@PlanId
	set @error=@error+@@error


	if(@Traveller is not null)
	begin
		declare @tdoc int
		exec sp_xml_preparedocument @tdoc output,@Traveller	
		INSERT INTO tbl_PlanPiaoTraveller
				   (TravellerId,PlanId,TourId,CompanyId,TravellerName
				   ,TravellerType,CardType,CardNumber,Birthday,Gender,Contact)
		select TravellerId,@PlanId,@TourId,@CompanyId,TravellerName,TravellerType,CardType,CardNumber,Birthday,Gender,Contact
		from openxml(@tdoc,'/Root/Traveller')
		with(TravellerId char(36),TravellerName nvarchar(50), TravellerType tinyint, 
             CardType tinyint, CardNumber nvarchar(50),Birthday nvarchar(100), Gender tinyint, Contact nvarchar(255))
		set @error=@error+@@error

		exec sp_xml_removedocument @tdoc
	end

	delete from tbl_PlanPiaoFile where PlanId=@PlanId
	set @error=@error+@@error

	if(@File is not null)
	begin
		declare @fdoc int
		exec sp_xml_preparedocument @fdoc output,@File

		INSERT INTO tbl_PlanPiaoFile(PlanId,[FileName],FilePath)
		select @PlanId,[FileName],FilePath
		from openxml(@fdoc,'/Root/File')
		with([FileName] nvarchar(255),FilePath nvarchar(255))
		set @error=@error+@@error

		exec sp_xml_removedocument @fdoc
	end
	
	if(@error=0)
	begin
		set @Result=1
		commit transaction
	end
	else
	begin
		set @Result=0
		rollback transaction
	end

	return @Result
end
GO

ALTER view [dbo].[view_Piao]
as
select 
A.PlanId,
A.CompanyId,
A.TourId,
A.TicketType,
A.TicketMode,
A.GysId,
A.IsMonth,
A.MonthTime,
B.TourCode,
B.IsEnd,
B.LDate,
B.TourType,
B.TourStatus,
(select UnitName from tbl_CompanySupplier where Id=A.GysId) as GysName ,
(select ContactName from tbl_CompanyUser where Id=A.TicketerId) as Ticketer,
A.TicketerId,
A.TrafficNumber,
A.TrafficTime,
A.Interval,
A.Adults,
A.Childs,
A.AdultPrice,
A.ChildPrice,
A.OtherPrice,
A.Brokerage,
A.TrafficSeat,
A.PayType,
A.SumPrice,
A.OperatorId,
A.IssueTime,
A.TicketStatus
from
tbl_PlanPiao as A
inner join
tbl_Tour as B
on A.TourId=B.TourId
GO

ALTER view [dbo].[view_DiJie]
as
SELECT A.PlanId
	  ,A.CompanyId
      ,A.GysId
	  ,(select UnitName from tbl_CompanySupplier where Id=A.GysId) as GysName--供应商名称
	  ,(select ContactName,ContactTel,QQ from tbl_SupplierContact where SupplierId=A.GysId for xml raw,root('Root')) as ContactInfo--计调信息
      ,A.Hotel
      ,A.Dining
      ,A.Car
      ,A.Ticket
      ,A.Guide
      ,A.Traffic
	  ,(select TicketType,Interval,TrafficTime,TrafficNumber from tbl_PlanPiao where TourId=A.TourId for xml raw,root('Root'))as PlanPiaoInfo --大交通信息
      ,A.Head
      ,A.AddPrice
      ,A.GuideIncome
      ,A.GuidePay
      ,A.Other
      ,A.SumPrice
      ,A.DiJieStatus
	  ,A.IssueTime
	  ,A.IsMonth
	  ,A.MonthTime
	  ,B.TourCode
	  ,B.TourType
	  ,B.RouteName
	  ,B.LDate
      ,B.Adults
      ,B.Childs
      ,B.Accompanys
	  ,B.TourStatus
	  ,B.IsEnd
  FROM tbl_PlanDiJie as A inner join tbl_Tour as B
  on A.TourId=B.TourId
GO





-- =============================================
-- Author:		<王磊>
-- Create date: <2013-04-09>
-- Description:	<修改线路>
-- Result :		1:成功 0：失败 -1:线路名称已存在
-- =============================================
Create proc [dbo].[proc_Route_Update]
@Id char(36),
@CompanyId int,
@RouteName nvarchar(250),
@Result int output
as
begin
	declare @error int
	set @error=0

	if exists(select 1 from tbl_Route where RouteName=@RouteName and CompanyId=@CompanyId and Id<>@Id)
	begin
		set @Result=-1
		return @Result
	end

	Update tbl_Route set RouteName=@RouteName where Id=@Id
	set @error=@error+@@error

	if(@error=0)
	begin
		set @Result=1
	end
	else
	begin
		set @Result=0
	end

	return @Result
	
end


Go
--修改TrafficSeat 硬座、软座、硬卧、软卧、高铁一等、高铁二等、商务舱、经济舱、头等舱	
alter table tbl_PlanPiao add _TrafficSeat tinyint not null default(0)
GO



-- =============================================
-- Author:		<王磊>
-- Create date: <2012-12-24>
-- Description:	<添加票务安排>
-- Result :		1:成功 0：失败 -1：供应商不存在
-- =============================================
ALTER proc [dbo].[proc_PlanPiao_Add]
@PlanId char(36),
@CompanyId int,
@TourId char(36),
@TicketType tinyint,
@TicketMode tinyint,
@GysId char(36),
@TicketerId int,
@TrafficNumber nvarchar(100),
@TrafficTime nvarchar(100),
@Interval nvarchar(100),
@Adults int,
@Childs int,
@AdultPrice money,
@ChildPrice money,
@OtherPrice money,
@Brokerage money,
@TrafficSeat nvarchar(100),
@_TrafficSeat tinyint,
@PayType tinyint,
@SumPrice money,
@OperatorId int,
@TicketStatus tinyint,
@IsMonth char(1),
@MonthTime datetime,
@Traveller xml,
@File xml,
@Result int output
as
begin
	declare @error int
	set @error=0

	if not exists(select 1 from tbl_CompanySupplier where @CompanyId=CompanyId and Id=@GysId)
	begin
		set @Result=-1	--供应商不存在
		return @Result
	end

	begin transaction 

	INSERT INTO tbl_PlanPiao
           (PlanId,CompanyId,TourId,TicketType,TicketMode
           ,GysId,TicketerId,TrafficNumber,TrafficTime,Interval
           ,Adults,Childs,AdultPrice,ChildPrice,OtherPrice
           ,Brokerage,TrafficSeat,_TrafficSeat,PayType,SumPrice,OperatorId,TicketStatus,IsMonth,MonthTime)
     VALUES(@PlanId,@CompanyId,@TourId,@TicketType,@TicketMode,
           @GysId,@TicketerId,@TrafficNumber,@TrafficTime,@Interval,
           @Adults,@Childs,@AdultPrice,@ChildPrice,@OtherPrice,
           @Brokerage,@TrafficSeat,@_TrafficSeat,@PayType,@SumPrice,@OperatorId,@TicketStatus,@IsMonth,@MonthTime)
	set @error=@error+@@error

	if(@Traveller is not null)
	begin
		declare @tdoc int
		exec sp_xml_preparedocument @tdoc output,@Traveller	


		INSERT INTO tbl_PlanPiaoTraveller
				   (TravellerId,PlanId,TourId,CompanyId,TravellerName
				   ,TravellerType,CardType,CardNumber,Birthday,Gender,Contact)
		select TravellerId,@PlanId,@TourId,@CompanyId,TravellerName,TravellerType,CardType,CardNumber,Birthday,Gender,Contact
		from openxml(@tdoc,'/Root/Traveller')
		with(TravellerId char(36),TravellerName nvarchar(50), TravellerType tinyint, 
             CardType tinyint, CardNumber nvarchar(50),Birthday nvarchar(100), Gender tinyint, Contact nvarchar(255))
		set @error=@error+@@error

		exec sp_xml_removedocument @tdoc
	end
	
	if(@File is not null)
	begin
		declare @fdoc int
		exec sp_xml_preparedocument @fdoc output,@File

		INSERT INTO tbl_PlanPiaoFile(PlanId,[FileName],FilePath)
		select @PlanId,[FileName],FilePath
		from openxml(@fdoc,'/Root/File')
		with([FileName] nvarchar(255),FilePath nvarchar(255))
		set @error=@error+@@error

		exec sp_xml_removedocument @fdoc
	end

	if(@error=0)
	begin
		set @Result=1
		commit transaction
	end
	else
	begin
		set @Result=0
		rollback transaction
	end

	return @Result
	
end

GO


-- =============================================
-- Author:		<王磊>
-- Create date: <2012-12-24>
-- Description:	<修改票务安排>
-- Result :-1:出票、退票状态审核后不能修改
--			-2:供应商不存在	
--		   1:成功 0：失败
-- =============================================
ALTER proc [dbo].[proc_PlanPiao_Update]
@PlanId char(36),
@CompanyId int,
@TourId char(36),
@TicketType tinyint,
@TicketMode tinyint,
@GysId char(36),
@TicketerId int,
@TrafficNumber nvarchar(100),
@TrafficTime nvarchar(100),
@Interval nvarchar(100),
@Adults int,
@Childs int,
@AdultPrice money,
@ChildPrice money,
@OtherPrice money,
@Brokerage money,
@TrafficSeat nvarchar(100),
@_TrafficSeat tinyint,
@PayType tinyint,
@SumPrice money,
@IsMonth char(1),
@MonthTime datetime,
@Traveller xml,
@File xml,
@Result int output
as
begin
	declare @error int
	set @error=0
	if exists(select 1 from tbl_PlanPiao where PlanId = @PlanId and TicketStatus=2)
	begin
		set @Result=-1	--出票、退票状态审核后不能修改
		return @Result
	end
	
	if not exists(select 1 from tbl_CompanySupplier where @CompanyId=CompanyId and Id=@GysId)
	begin
		set @Result=-2	--供应商不存在
		return @Result
	end
	
	begin transaction
	UPDATE tbl_PlanPiao
	   SET TicketType = @TicketType, TicketMode = @TicketMode 
		  ,GysId = @GysId,TicketerId = @TicketerId,TrafficNumber = @TrafficNumber
		  ,TrafficTime = @TrafficTime,Interval = @Interval
		  ,Adults = @Adults,Childs = @Childs,AdultPrice = @AdultPrice
		  ,ChildPrice = @ChildPrice,OtherPrice = @OtherPrice
		  ,Brokerage = @Brokerage,TrafficSeat = @TrafficSeat,_TrafficSeat=@_TrafficSeat
		  ,PayType = @PayType,SumPrice = @SumPrice,IsMonth=@IsMonth,MonthTime=@MonthTime
	 WHERE PlanId = @PlanId
	set @error=@error+@@error

	delete from tbl_PlanPiaoTraveller where PlanId=@PlanId
	set @error=@error+@@error


	if(@Traveller is not null)
	begin
		declare @tdoc int
		exec sp_xml_preparedocument @tdoc output,@Traveller	
		INSERT INTO tbl_PlanPiaoTraveller
				   (TravellerId,PlanId,TourId,CompanyId,TravellerName
				   ,TravellerType,CardType,CardNumber,Birthday,Gender,Contact)
		select TravellerId,@PlanId,@TourId,@CompanyId,TravellerName,TravellerType,CardType,CardNumber,Birthday,Gender,Contact
		from openxml(@tdoc,'/Root/Traveller')
		with(TravellerId char(36),TravellerName nvarchar(50), TravellerType tinyint, 
             CardType tinyint, CardNumber nvarchar(50),Birthday nvarchar(100), Gender tinyint, Contact nvarchar(255))
		set @error=@error+@@error

		exec sp_xml_removedocument @tdoc
	end

	delete from tbl_PlanPiaoFile where PlanId=@PlanId
	set @error=@error+@@error

	if(@File is not null)
	begin
		declare @fdoc int
		exec sp_xml_preparedocument @fdoc output,@File

		INSERT INTO tbl_PlanPiaoFile(PlanId,[FileName],FilePath)
		select @PlanId,[FileName],FilePath
		from openxml(@fdoc,'/Root/File')
		with([FileName] nvarchar(255),FilePath nvarchar(255))
		set @error=@error+@@error

		exec sp_xml_removedocument @fdoc
	end
	
	if(@error=0)
	begin
		set @Result=1
		commit transaction
	end
	else
	begin
		set @Result=0
		rollback transaction
	end

	return @Result
end

GO


alter view [dbo].[view_Piao]
as
select 
A.PlanId,
A.CompanyId,
A.TourId,
A.TicketType,
A.TicketMode,
A.GysId,
A.IsMonth,
A.MonthTime,
B.TourCode,
B.IsEnd,
B.LDate,
B.TourType,
B.TourStatus,
(select UnitName from tbl_CompanySupplier where Id=A.GysId) as GysName ,
(select ContactName from tbl_CompanyUser where Id=A.TicketerId) as Ticketer,
A.TicketerId,
A.TrafficNumber,
A.TrafficTime,
A.Interval,
A.Adults,
A.Childs,
A.AdultPrice,
A.ChildPrice,
A.OtherPrice,
A.Brokerage,
A.TrafficSeat,
A._TrafficSeat,
A.PayType,
A.SumPrice,
A.OperatorId,
A.IssueTime,
A.TicketStatus
from
tbl_PlanPiao as A
inner join
tbl_Tour as B
on A.TourId=B.TourId
Go



go

alter table tbl_CompanyTicket add _TrafficSeat tinyint not null default(0)
	
GO
	
	

GO

-- =============================================
-- Author:		<周文超>
-- Create date: <2012-12-28>
-- Description:	<团散统计列表>
-- =============================================
ALTER view [dbo].[view_TourAndCustomer]
as

SELECT a.BuyCompanyId AS CustomerId,a.LDate,a.TourCode,a.Adults,a.Childs,a.Accompanys,a.RouteName,a.SumPrice,a.Profit,a.TourType,a.IssueTime,a.SaleId,a.OperatorId,a.TourId,a.CompanyId,a.IsDelete,a.ReceivedMoney,a.CheckMoney
,(ISNULL(a.RebatePeople,0) * ISNULL(a.RebatePrice,0)) AS YongJin
,b.CustomerName,b.ProviceId AS CustomerProviceId,b.CityId AS CustomerCityId,b.ContactName,b.Phone AS ContactTel,b.Mobile AS ContactMobile,b.IssueTime AS CustomerIssueTime,b.SaleAreadId
,c.ContactName AS OperatorName
,d.ContactName AS SaleName
,e.ProvinceName AS CustomerProvinceName
,f.CityName AS CustomerCityName
,g.SaleAreaName
,(SELECT ISNULL(SUM(pd.SumPrice),0) FROM tbl_PlanDiJie AS pd WHERE pd.TourId = a.TourId) AS DiJieZhiChu
,(SELECT ISNULL(SUM(pp.SumPrice),0) FROM tbl_PlanPiao AS pp WHERE pp.TourId = a.TourId) AS JiPiaoZhiChu
FROM 
tbl_Tour AS a LEFT JOIN tbl_Customer AS b ON a.BuyCompanyId = b.Id
LEFT JOIN tbl_CompanyUser AS c on a.OperatorId = c.Id
LEFT JOIN tbl_CompanyUser AS d ON a.SaleId = d.Id
LEFT JOIN tbl_CompanyProvince AS e on e.Id = b.ProviceId
LEFT JOIN tbl_CompanyCity AS f ON f.Id = b.CityId
LEFT JOIN tbl_CompanySaleArea AS g ON g.SaleAreaId = b.SaleAreadId
WHERE a.IsDelete = '0'

go


GO


-- =============================================
-- Author:		<王磊>
-- Create date: <2012-12-24>
-- Description:	<添加票务安排>
-- Result :		1:成功 0：失败 -1：供应商不存在
-- =============================================
ALTER proc [dbo].[proc_PlanPiao_Add]
@PlanId char(36),
@CompanyId int,
@TourId char(36),
@TicketType tinyint,
@TicketMode tinyint,
@GysId char(36),
@TicketerId int,
@TrafficNumber nvarchar(100),
@TrafficTime nvarchar(100),
@Interval nvarchar(100),
@Adults int,
@Childs int,
@AdultPrice money,
@ChildPrice money,
@OtherPrice money,
@Brokerage money,
@TrafficSeat nvarchar(100),
@_TrafficSeat tinyint,
@PayType tinyint,
@SumPrice money,
@OperatorId int,
@TicketStatus tinyint,
@IsMonth char(1),
@MonthTime datetime,
@Traveller xml,
@File xml,
@Result int output
as
begin
	declare @error int
	set @error=0

--	if not exists(select 1 from tbl_CompanySupplier where @CompanyId=CompanyId and Id=@GysId)
--	begin
--		set @Result=-1	--供应商不存在
--		return @Result
--	end

	begin transaction 

	INSERT INTO tbl_PlanPiao
           (PlanId,CompanyId,TourId,TicketType,TicketMode
           ,GysId,TicketerId,TrafficNumber,TrafficTime,Interval
           ,Adults,Childs,AdultPrice,ChildPrice,OtherPrice
           ,Brokerage,TrafficSeat,_TrafficSeat,PayType,SumPrice,OperatorId,TicketStatus,IsMonth,MonthTime)
     VALUES(@PlanId,@CompanyId,@TourId,@TicketType,@TicketMode,
           @GysId,@TicketerId,@TrafficNumber,@TrafficTime,@Interval,
           @Adults,@Childs,@AdultPrice,@ChildPrice,@OtherPrice,
           @Brokerage,@TrafficSeat,@_TrafficSeat,@PayType,@SumPrice,@OperatorId,@TicketStatus,@IsMonth,@MonthTime)
	set @error=@error+@@error

	if(@Traveller is not null)
	begin
		declare @tdoc int
		exec sp_xml_preparedocument @tdoc output,@Traveller	


		INSERT INTO tbl_PlanPiaoTraveller
				   (TravellerId,PlanId,TourId,CompanyId,TravellerName
				   ,TravellerType,CardType,CardNumber,Birthday,Gender,Contact)
		select TravellerId,@PlanId,@TourId,@CompanyId,TravellerName,TravellerType,CardType,CardNumber,Birthday,Gender,Contact
		from openxml(@tdoc,'/Root/Traveller')
		with(TravellerId char(36),TravellerName nvarchar(50), TravellerType tinyint, 
             CardType tinyint, CardNumber nvarchar(50),Birthday nvarchar(100), Gender tinyint, Contact nvarchar(255))
		set @error=@error+@@error

		exec sp_xml_removedocument @tdoc
	end
	
	if(@File is not null)
	begin
		declare @fdoc int
		exec sp_xml_preparedocument @fdoc output,@File

		INSERT INTO tbl_PlanPiaoFile(PlanId,[FileName],FilePath)
		select @PlanId,[FileName],FilePath
		from openxml(@fdoc,'/Root/File')
		with([FileName] nvarchar(255),FilePath nvarchar(255))
		set @error=@error+@@error

		exec sp_xml_removedocument @fdoc
	end

	if(@error=0)
	begin
		set @Result=1
		commit transaction
	end
	else
	begin
		set @Result=0
		rollback transaction
	end

	return @Result
	
end


go




GO






-- =============================================
-- Author:		<王磊>
-- Create date: <2012-12-24>
-- Description:	<添加地接安排>
-- Result :		1:成功 0：失败 -1:供应商不存在
-- =============================================
ALTER proc [dbo].[proc_Dijie_Add]
@PlanId char(36),
@CompanyId int,
@TourId nvarchar(50),
@GysId char(36),
@Hotel money,
@Dining money,
@Car money,
@Ticket money,
@Guide money,
@Traffic money,
@Head money,
@AddPrice money,
@GuideIncome money,
@GuidePay money,
@Other money,
@PayType tinyint,
@SumPrice money,
@Remark nvarchar(255),
@OperatorId int,
@DiJieStatus tinyint,
@IsMonth char(1),
@MonthTime datetime,
@File xml,
@Result int output
as
begin
	declare @error int
	set @error=0
--	if not exists(select 1 from tbl_CompanySupplier where @CompanyId=CompanyId and Id=@GysId)
--	begin
--		set @Result=-1	--供应商不存在
--		return @Result
--	end

	begin transaction
	INSERT INTO tbl_PlanDiJie
           (PlanId,CompanyId,TourId,GysId,Hotel,Dining,Car,Ticket
           ,Guide,Traffic,Head,AddPrice,GuideIncome,GuidePay
           ,Other,PayType,SumPrice,Remark,OperatorId,DiJieStatus,IsMonth,MonthTime)
     VALUES
           (@PlanId ,@CompanyId,@TourId,@GysId,@Hotel,@Dining,@Car,@Ticket
           ,@Guide,@Traffic,@Head,@AddPrice,@GuideIncome,@GuidePay
           ,@Other,@PayType,@SumPrice,@Remark,@OperatorId,@DiJieStatus,@IsMonth,@MonthTime)
	set @error=@error+@@error

	if(@File is not null)
	begin
		declare @fdoc int
		exec sp_xml_preparedocument @fdoc output,@File

		INSERT INTO tbl_PlanDiJieFile(PlanId,[FileName],FilePath)
		select @PlanId,[FileName],FilePath
		from openxml(@fdoc,'/Root/File')
		with([FileName] nvarchar(255),FilePath nvarchar(255))
		set @error=@error+@@error

		exec sp_xml_removedocument @fdoc
	end

	if(@error=0)
	begin
		set @Result=1
		commit transaction
	end
	else
	begin
		set @Result=0
		rollback transaction
	end

	return @Result
end




go
-- =============================================
-- Author:		<王磊>
-- Create date: <2012-12-21>
-- Description:	<生成控位号>
-- Result :系统根据出团日期自动生成控位号，出团日期-当天流水号（T/S）生成
-- =============================================
ALTER function [dbo].[fn_TourCode](
	@CompanyId int,		--公司号
	@TourType tinyint,	--团队类型
	@LDate datetime,	--出团时间
	@_Type tinyint		-- 0:add or 1:update
)
RETURNS varchar(200)
begin
	--Select CONVERT(varchar(100), @LDate, 112)
	declare @mark nvarchar(10)
	set @mark='T'
	if(@TourType=1)
	begin
		set @mark='S'
	end
	
	declare @code nvarchar(500)
	declare @index int
	if(@_Type=0)
	begin
		if not exists(select 1 from tbl_Tour  where CompanyId=@CompanyId and LDate=@LDate )
		begin
			select @index=count(1)+1 from tbl_Tour  where CompanyId=@CompanyId and LDate=@LDate 
		end
		else
		begin
			select @index=isnull(max(subString(TourCode,len(TourCode)-3,3)),0)+1  from tbl_Tour where CompanyId=@CompanyId and LDate=@LDate
		end
	end
	if(@_Type=1)
	begin
		 select @index=isnull(max(subString(TourCode,len(TourCode)-3,3)),0)+1  from tbl_Tour where CompanyId=@CompanyId and LDate=@LDate
		
	end

	if(@index>=1 and @index<=9)
	begin
		set @code=substring(CONVERT(varchar(100), @LDate, 112),3,6)+'00'+cast(@index as nvarchar(10))+@mark
	end
	else if(@index>=10 and @index<=99)
	begin
		set @code=substring(CONVERT(varchar(100), @LDate, 112),3,6)+'0'+cast(@index as nvarchar(10))+@mark
	end
	else
	begin
		set @code=substring(CONVERT(varchar(100), @LDate, 112),3,6)+cast(@index as varchar(10))+@mark
	end
	
	return (@code)
end
GO






go

--隐藏团队质量管理的权限，将质量评估权限改为团队回访下
UPDATE [tbl_SysPrivs3] SET ParentId = 4 WHERE id = 29
  
UPDATE tbl_SysPrivs2 SET IsEnable = 0 WHERE Id = 5
 
UPDATE [tbl_SysPrivs3] SET IsEnable = 0 WHERE ParentId = 5


go
GO

--批量将TrafficSeat 更改为_TrafficSeat的枚举
declare @PlanId char(36)
declare @TrafficSeat nvarchar(100)
declare cursor_for_seat cursor for select PlanId,TrafficSeat from tbl_PlanPiao
open cursor_for_seat
fetch next from  cursor_for_seat  into @PlanId,@TrafficSeat
while @@fetch_status=0
begin
	
	if(len(@TrafficSeat)=0 or @TrafficSeat is null)
	begin
		update tbl_PlanPiao set _TrafficSeat=0 where PlanId=@PlanId
	end	
	else if(@TrafficSeat='硬座')
	begin
		update tbl_PlanPiao set _TrafficSeat=1 where PlanId=@PlanId
	end
	else if(@TrafficSeat='软座')
	begin
		 update tbl_PlanPiao set _TrafficSeat=2 where PlanId=@PlanId
	end
	else if(@TrafficSeat='硬卧')
	begin
		update tbl_PlanPiao set _TrafficSeat=3 where PlanId=@PlanId
	end
	else if(@TrafficSeat='软卧')
	begin
		update tbl_PlanPiao set _TrafficSeat=4 where PlanId=@PlanId
	end
	else if(@TrafficSeat='高铁一等')
	begin
		update tbl_PlanPiao set _TrafficSeat=5 where PlanId=@PlanId
	end
	else if(@TrafficSeat='高铁二等')
	begin
		update tbl_PlanPiao set _TrafficSeat=6 where PlanId=@PlanId
	end
	else if(@TrafficSeat='商务舱')
	begin
		update tbl_PlanPiao set _TrafficSeat=7 where PlanId=@PlanId
	end
	else if(@TrafficSeat='经济舱')
	begin
		update tbl_PlanPiao set _TrafficSeat=8 where PlanId=@PlanId
	end
	else if(@TrafficSeat='头等舱')
	begin
		update tbl_PlanPiao set _TrafficSeat=9 where PlanId=@PlanId
	end
	else if(CHARINDEX('一',@TrafficSeat)<>0)
	begin
		update tbl_PlanPiao set _TrafficSeat=5 where PlanId=@PlanId
	end
	else if(CHARINDEX('二',@TrafficSeat)<>0)
	begin
		update tbl_PlanPiao set _TrafficSeat=6 where PlanId=@PlanId
	end
	else if(CHARINDEX('硬卧',@TrafficSeat)<>0)
	begin
		update tbl_PlanPiao set _TrafficSeat=3 where PlanId=@PlanId
	end
	else if(CHARINDEX('软卧',@TrafficSeat)<>0)
	begin
		update tbl_PlanPiao set _TrafficSeat=4 where PlanId=@PlanId
	end
	else if(CHARINDEX('经济',@TrafficSeat)<>0)
	begin
		update tbl_PlanPiao set _TrafficSeat=8 where PlanId=@PlanId
	end
	else if(CHARINDEX('头等',@TrafficSeat)<>0)
	begin
		update tbl_PlanPiao set _TrafficSeat=9 where PlanId=@PlanId
	end
	else 
	begin
		update tbl_PlanPiao set _TrafficSeat=0 where PlanId=@PlanId
	end
	
	
	fetch next from  cursor_for_seat  into @PlanId,@TrafficSeat
end

deallocate  cursor_for_seat 

GO

------------------------------2013-04-19--------------------------------------------------------

update tbl_PlanPiao  set TrafficTime=null where ISDATE (TrafficTime)=0
GO

alter table tbl_PlanPiao alter column TrafficTime datetime
GO



-- =============================================
-- Author:		<王磊>
-- Create date: <2012-12-24>
-- Description:	<添加票务安排>
-- Result :		1:成功 0：失败 -1：供应商不存在
-- =============================================
ALTER proc [dbo].[proc_PlanPiao_Add]
@PlanId char(36),
@CompanyId int,
@TourId char(36),
@TicketType tinyint,
@TicketMode tinyint,
@GysId char(36),
@TicketerId int,
@TrafficNumber nvarchar(100),
@TrafficTime datetime,
@Interval nvarchar(100),
@Adults int,
@Childs int,
@AdultPrice money,
@ChildPrice money,
@OtherPrice money,
@Brokerage money,
@TrafficSeat nvarchar(100),
@_TrafficSeat tinyint,
@PayType tinyint,
@SumPrice money,
@OperatorId int,
@TicketStatus tinyint,
@IsMonth char(1),
@MonthTime datetime,
@Traveller xml,
@File xml,
@Result int output
as
begin
	declare @error int
	set @error=0

--	if not exists(select 1 from tbl_CompanySupplier where @CompanyId=CompanyId and Id=@GysId)
--	begin
--		set @Result=-1	--供应商不存在
--		return @Result
--	end

	begin transaction 

	INSERT INTO tbl_PlanPiao
           (PlanId,CompanyId,TourId,TicketType,TicketMode
           ,GysId,TicketerId,TrafficNumber,TrafficTime,Interval
           ,Adults,Childs,AdultPrice,ChildPrice,OtherPrice
           ,Brokerage,TrafficSeat,_TrafficSeat,PayType,SumPrice,OperatorId,TicketStatus,IsMonth,MonthTime)
     VALUES(@PlanId,@CompanyId,@TourId,@TicketType,@TicketMode,
           @GysId,@TicketerId,@TrafficNumber,@TrafficTime,@Interval,
           @Adults,@Childs,@AdultPrice,@ChildPrice,@OtherPrice,
           @Brokerage,@TrafficSeat,@_TrafficSeat,@PayType,@SumPrice,@OperatorId,@TicketStatus,@IsMonth,@MonthTime)
	set @error=@error+@@error

	if(@Traveller is not null)
	begin
		declare @tdoc int
		exec sp_xml_preparedocument @tdoc output,@Traveller	


		INSERT INTO tbl_PlanPiaoTraveller
				   (TravellerId,PlanId,TourId,CompanyId,TravellerName
				   ,TravellerType,CardType,CardNumber,Birthday,Gender,Contact)
		select TravellerId,@PlanId,@TourId,@CompanyId,TravellerName,TravellerType,CardType,CardNumber,Birthday,Gender,Contact
		from openxml(@tdoc,'/Root/Traveller')
		with(TravellerId char(36),TravellerName nvarchar(50), TravellerType tinyint, 
             CardType tinyint, CardNumber nvarchar(50),Birthday nvarchar(100), Gender tinyint, Contact nvarchar(255))
		set @error=@error+@@error

		exec sp_xml_removedocument @tdoc
	end
	
	if(@File is not null)
	begin
		declare @fdoc int
		exec sp_xml_preparedocument @fdoc output,@File

		INSERT INTO tbl_PlanPiaoFile(PlanId,[FileName],FilePath)
		select @PlanId,[FileName],FilePath
		from openxml(@fdoc,'/Root/File')
		with([FileName] nvarchar(255),FilePath nvarchar(255))
		set @error=@error+@@error

		exec sp_xml_removedocument @fdoc
	end

	if(@error=0)
	begin
		set @Result=1
		commit transaction
	end
	else
	begin
		set @Result=0
		rollback transaction
	end

	return @Result
	
end
Go




-- =============================================
-- Author:		<王磊>
-- Create date: <2012-12-24>
-- Description:	<修改票务安排>
-- Result :-1:出票、退票状态审核后不能修改
--			-2:供应商不存在	
--		   1:成功 0：失败
-- =============================================
ALTER proc [dbo].[proc_PlanPiao_Update]
@PlanId char(36),
@CompanyId int,
@TourId char(36),
@TicketType tinyint,
@TicketMode tinyint,
@GysId char(36),
@TicketerId int,
@TrafficNumber nvarchar(100),
@TrafficTime datetime,
@Interval nvarchar(100),
@Adults int,
@Childs int,
@AdultPrice money,
@ChildPrice money,
@OtherPrice money,
@Brokerage money,
@TrafficSeat nvarchar(100),
@_TrafficSeat tinyint,
@PayType tinyint,
@SumPrice money,
@IsMonth char(1),
@MonthTime datetime,
@Traveller xml,
@File xml,
@Result int output
as
begin
	declare @error int
	set @error=0
	if exists(select 1 from tbl_PlanPiao where PlanId = @PlanId and TicketStatus=2)
	begin
		set @Result=-1	--出票、退票状态审核后不能修改
		return @Result
	end
	
	if not exists(select 1 from tbl_CompanySupplier where @CompanyId=CompanyId and Id=@GysId)
	begin
		set @Result=-2	--供应商不存在
		return @Result
	end
	
	begin transaction
	UPDATE tbl_PlanPiao
	   SET TicketType = @TicketType, TicketMode = @TicketMode 
		  ,GysId = @GysId,TicketerId = @TicketerId,TrafficNumber = @TrafficNumber
		  ,TrafficTime = @TrafficTime,Interval = @Interval
		  ,Adults = @Adults,Childs = @Childs,AdultPrice = @AdultPrice
		  ,ChildPrice = @ChildPrice,OtherPrice = @OtherPrice
		  ,Brokerage = @Brokerage,TrafficSeat = @TrafficSeat,_TrafficSeat=@_TrafficSeat
		  ,PayType = @PayType,SumPrice = @SumPrice,IsMonth=@IsMonth,MonthTime=@MonthTime
	 WHERE PlanId = @PlanId
	set @error=@error+@@error

	delete from tbl_PlanPiaoTraveller where PlanId=@PlanId
	set @error=@error+@@error


	if(@Traveller is not null)
	begin
		declare @tdoc int
		exec sp_xml_preparedocument @tdoc output,@Traveller	
		INSERT INTO tbl_PlanPiaoTraveller
				   (TravellerId,PlanId,TourId,CompanyId,TravellerName
				   ,TravellerType,CardType,CardNumber,Birthday,Gender,Contact)
		select TravellerId,@PlanId,@TourId,@CompanyId,TravellerName,TravellerType,CardType,CardNumber,Birthday,Gender,Contact
		from openxml(@tdoc,'/Root/Traveller')
		with(TravellerId char(36),TravellerName nvarchar(50), TravellerType tinyint, 
             CardType tinyint, CardNumber nvarchar(50),Birthday nvarchar(100), Gender tinyint, Contact nvarchar(255))
		set @error=@error+@@error

		exec sp_xml_removedocument @tdoc
	end

	delete from tbl_PlanPiaoFile where PlanId=@PlanId
	set @error=@error+@@error

	if(@File is not null)
	begin
		declare @fdoc int
		exec sp_xml_preparedocument @fdoc output,@File

		INSERT INTO tbl_PlanPiaoFile(PlanId,[FileName],FilePath)
		select @PlanId,[FileName],FilePath
		from openxml(@fdoc,'/Root/File')
		with([FileName] nvarchar(255),FilePath nvarchar(255))
		set @error=@error+@@error

		exec sp_xml_removedocument @fdoc
	end
	
	if(@error=0)
	begin
		set @Result=1
		commit transaction
	end
	else
	begin
		set @Result=0
		rollback transaction
	end

	return @Result
end


GO

alter table tbl_CompanyTicket alter column TrafficTime datetime 
Go

USE [hdfcTestDB]
GO
/****** 对象:  StoredProcedure [dbo].[proc_Tour_Fin_Update]    脚本日期: 06/26/2013 13:00:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<王磊>
-- Create date: <2012-12-21>
-- Description:	<财务保存操作>
-- Result :-1:只有未处理的订单才能修改
--			1:成功 0：失败
-- =============================================
ALTER proc [dbo].[proc_Tour_Fin_Update]
@TourId char(36), 
@CompanyId int, 
@RouteId char(36), 
@RouteName nvarchar(200), 
@IsRouteHuiTian char(1),
@LDate datetime, 
@RDate datetime, 
@SaleId int, 
@IsMonth char(1), 
@MonthTime datetime, 
@Adults int, 
@Childs int, 
@Accompanys int, 
@BuyCompanyId char(36), 
@SumPrice money, 
@IsChuPiao char(1), 
@Remark nvarchar(max),
@RebatePeople int,
@RebatePrice money,
@Profit money,
@ConfirmOperatorId int,--财务确认人
@ConfirmTime datetime,--财务确认时间	
@TourDiJie xml,--地接信息
@TourGuide xml,--导游信息
@File xml,	   --附件
@Result int output
as
begin
	
--	if not exists(select  1 from tbl_Tour where TourId=@TourId and TourStatus = 0)
--	begin
--		set @Result=-1	--财务已确认
--		return @Result
--	end

	
	
	declare @error int
	set @error=0
	
	begin transaction
		--回填线路信息
	IF(@IsRouteHuiTian='1')
	begin
		if exists(select 1 from tbl_Route where RouteName=@RouteName and CompanyId=@CompanyId)
		begin
			select @RouteId=Id from tbl_Route where RouteName=@RouteName and CompanyId=@CompanyId
		end
		else
		begin
			set @RouteId=newId()
			INSERT INTO tbl_Route(Id,CompanyId,RouteName)VALUES(@RouteId,@CompanyId,@RouteName)
			set @error=@error+@@error
		end
	end	

	declare @TourStatus tinyint
	select @TourStatus=TourStatus from tbl_Tour where TourId=@TourId
	if(@TourStatus=0)
	begin
		begin
			if (DATEDIFF(dd,getdate(),@LDate)<=0 AND DATEDIFF(dd,getdate(),@RDate)>0)
			set @TourStatus=2
			else if (DATEDIFF(dd,getdate(),@RDate)<=0)
			set @TourStatus=3
			else
			set	@TourStatus=1
		end
	end

	--财务保存操作将计划状态变为 未出发状态
	UPDATE tbl_Tour SET RouteId = @RouteId,RouteName = @RouteName,
	LDate = @LDate,RDate = @RDate,SaleId = @SaleId, IsMonth = @IsMonth, 
	MonthTime = @MonthTime,Adults = @Adults, Childs = @Childs,
	Accompanys = @Accompanys,BuyCompanyId = @BuyCompanyId,
	SumPrice = @SumPrice,IsChuPiao = @IsChuPiao ,Remark=@Remark,
	RebatePeople=@RebatePeople,RebatePrice=@RebatePrice,Profit=@Profit,
	TourStatus=@TourStatus,ConfirmOperatorId=@ConfirmOperatorId,ConfirmTime=@ConfirmTime
	WHERE TourId=@TourId
	set @error=@error+@@error
	
	--删除导游信息
	delete from tbl_TourGuide where TourId=@TourId
	set @error=@error+@@error

	if(@TourGuide is not null)
	begin
		declare @dydoc int
		exec sp_xml_preparedocument @dydoc output,@TourGuide

		INSERT INTO tbl_TourGuide(Id,TourId,GuideId,Phone)
		select Id,@TourId,GuideId,Phone from openxml(@dydoc,'/Root/TourGuide')
		with(Id char(36),GuideId char(36),Phone nvarchar(20))
		set @error=@error+@@error
		
		exec sp_xml_removedocument @dydoc
	end
	
	--删除附件
	delete from tbl_TourFile where TourId=@TourId
	
	if(@File is not null)
	begin
		declare @fdoc int
		exec sp_xml_preparedocument @fdoc output,@File

		INSERT INTO tbl_TourFile(Id,TourId,[FileName],FilePath)
		select Id,@TourId,[FileName],FilePath
		from openxml(@fdoc,'/Root/File')
		with(Id char(36),[FileName] nvarchar(255),FilePath nvarchar(255))
		set @error=@error+@@error
		exec sp_xml_removedocument @fdoc
	end

	if(@error=0)
	begin
		set @Result=1
		commit transaction
	end	
	else
	begin
		set @Result=0
		rollback transaction
	end

	return @Result
end


-- =============================================
-- Author:		<邓保朝>
-- Create date: <2013-09-30>
-- Description:	<信用等级操作>
-- =============================================

--tbl_Customer 添加字段
alter table tbl_Customer  add RatingId int null 

--添加信用等级表
CREATE TABLE [tbl_YingyongRating] (
[Id] [int]  IDENTITY (1, 1)  NOT NULL,
[RatingName] [nvarchar]  (255) NOT NULL,
[CompanyId] [int]  NOT NULL,
[OperatorId] [int]  NOT NULL,
[IssueTime] [datetime]  NOT NULL DEFAULT (getdate()),
[IsDelete] [char]  (1) NOT NULL,
[SortId] [int]  NOT NULL)

--添加 tbl_SysPrivs3 数据

INSERT [tbl_SysPrivs3] ([Id],[ParentId],[Name],[SortId],[IsEnable],[PrivsType]) VALUES ( 143,29,N'信用等级栏目',0,N'1',0)









































