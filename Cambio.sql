CREATE DATABASE CAMBIO
GO

USE CAMBIO
GO

create table ROL(
IdRol int primary key identity,
Descripcion varchar(50),
FechaRegistro datetime default getdate()
)

go

create table PERMISO(
IdPermiso int primary key identity,
IdRol int references ROL(IdRol),
NombreMenu varchar(100),
Estado bit,
FechaRegistro datetime default getdate()
)

go

create table PROVEEDOR(
IdProveedor int primary key identity,
Documento varchar(50),
Banco varchar(50),
Correo varchar(50),
Telefono varchar(50),
Estado bit,
FechaRegistro datetime default getdate()
)

go

create table CLIENTE(
IdCliente int primary key identity,
Documento varchar(50),
NombreCompleto varchar(50),
Correo varchar(50),
Telefono varchar(50),
Estado bit,
FechaRegistro datetime default getdate()
)

go

create table USUARIO(
IdUsuario int primary key identity,
Documento varchar(50),
NombreCompleto varchar(50),
Correo varchar(50),
Clave varchar(50),
IdRol int references ROL(IdRol),
Estado bit,
FechaRegistro datetime default getdate()
)

go

create table CATEGORIA(
IdCategoria int primary key identity,
Pais varchar(100),
Estado bit,
FechaRegistro datetime default getdate()
)

go

create table MONERA(
IdMonera int primary key identity,
Codigo varchar(50),
Nombre varchar(50),
Descripcion varchar(50),
IdCategoria int references CATEGORIA(IdCategoria),
Stock int not null default 0,
PrecioCompra decimal(10,2) default 0,
PrecioVenta decimal(10,2) default 0,
Estado bit,
FechaRegistro datetime default getdate()
)

go

create table COMPRA(
IdCompra int primary key identity,
IdUsuario int references USUARIO(IdUsuario),
IdProveedor int references PROVEEDOR(IdProveedor),
TipoDocumento varchar(50),
NumeroDocumento varchar(50),
MontoTotal decimal(10,2),
FechaRegistro datetime default getdate()
)


go

create table DETALLE_COMPRA(
IdDetalleCompra int primary key identity,
IdCompra int references COMPRA(IdCompra),
IdMonera int references MONERA(IdMonera),
PrecioCompra decimal(10,2) default 0,
PrecioVenta decimal(10,2) default 0,
Cantidad int,
MontoTotal decimal(10,2),
FechaRegistro datetime default getdate()
)

go

create table VENTA(
IdVenta int primary key identity,
IdUsuario int references USUARIO(IdUsuario),
TipoDocumento varchar(50),
NumeroDocumento varchar(50),
DocumentoCliente varchar(50),
NombreCliente varchar(100),
MontoPago decimal(10,2),
MontoCambio decimal(10,2),
MontoTotal decimal(10,2),
FechaRegistro datetime default getdate()
)


go


create table DETALLE_VENTA(
IdDetalleVenta int primary key identity,
IdVenta int references VENTA(IdVenta),
IdMonera int references MONERA(IdMonera),
PrecioVenta decimal(10,2),
Cantidad int,
SubTotal decimal(10,2),
FechaRegistro datetime default getdate()
)

go

create table NEGOCIO(
IdNegocio int primary key,
Nombre varchar(60),
RFC varchar(60),
Direccion varchar(60),
Telefono varchar(60),
Correo varchar(60),
Logo varbinary(max) NULL
)

go

----------------------------------------------------CLIENTE--------------------------------------------------------------------------------

create PROC sp_RegistroUsuario(
@Documento varchar(50),
@NombreCompleto varchar(100),
@Correo varchar(100),
@Clave varchar(100),
@IdRol int,
@Estado bit,
@IdUsuarioResultado int output,
@Mensaje varchar(500) output
)
as
begin
	set @IdUsuarioResultado = 0
	set @Mensaje = ''


	if not exists(select * from USUARIO where Documento = @Documento)
	begin
		insert into USUARIO(Documento,NombreCompleto,Correo,Clave,IdRol,Estado) values
		(@Documento,@NombreCompleto,@Correo,@Clave,@IdRol,@Estado)

		set @IdUsuarioResultado = SCOPE_IDENTITY()
		
	end
	else
		set @Mensaje = 'No se puede repetir el usuario'


end

go

create PROC sp_EditarUsuario(
@IdUsuario int,
@Documento varchar(50),
@NombreCompleto varchar(100),
@Correo varchar(100),
@Clave varchar(100),
@IdRol int,
@Estado bit,
@Respuesta bit output,
@Mensaje varchar(500) output
)
as
begin
	set @Respuesta = 0
	set @Mensaje = ''


	if not exists(select * from USUARIO where Documento = @Documento and idusuario != @IdUsuario)
	begin
		update  usuario set
		Documento = @Documento,
		NombreCompleto = @NombreCompleto,
		Correo = @Correo,
		Clave = @Clave,
		IdRol = @IdRol,
		Estado = @Estado
		where IdUsuario = @IdUsuario

		set @Respuesta = 1
		
	end
	else
		set @Mensaje = 'No se puede repetir el usuario'


end
go

create PROC sp_EliminarUsuario(
@IdUsuario int,
@Respuesta bit output,
@Mensaje varchar(500) output
)
as
begin
	set @Respuesta = 0
	set @Mensaje = ''
	declare @pasoreglas bit = 1

	IF EXISTS (SELECT * FROM COMPRA C 
	INNER JOIN USUARIO U ON U.IdUsuario = C.IdUsuario
	WHERE U.IDUSUARIO = @IdUsuario
	)
	BEGIN
		set @pasoreglas = 0
		set @Respuesta = 0
		set @Mensaje = @Mensaje + 'No se puede eliminar, el usuario, se encuentra relacionado a una COMPRA\n' 
	END

	IF EXISTS (SELECT * FROM VENTA V
	INNER JOIN USUARIO U ON U.IdUsuario = V.IdUsuario
	WHERE U.IDUSUARIO = @IdUsuario
	)
	BEGIN
		set @pasoreglas = 0
		set @Respuesta = 0
		set @Mensaje = @Mensaje + 'No se puede eliminar, el usuario se encuentra relacionado a una VENTA\n' 
	END

	if(@pasoreglas = 1)
	begin
		delete from USUARIO where IdUsuario = @IdUsuario
		set @Respuesta = 1 
	end

end

go

---------------------------------------------------------CATEGORIA-------------------------------------------------------------------------------------


create PROC SP_RegistrarCategoria(
@Pais varchar(50),
@Estado bit,
@Resultado int output,
@Mensaje varchar(500) output
)as
begin
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM CATEGORIA WHERE Pais = @Pais)
	begin
		insert into CATEGORIA(Pais,Estado) values (@Pais,@Estado)
		set @Resultado = SCOPE_IDENTITY()
	end
	ELSE
		set @Mensaje = 'No se puede repetir la descripcion de una categoria'
	
end


go

Create procedure sp_EditarCategoria(
@IdCategoria int,
@Pais varchar(50),
@Estado bit,
@Resultado bit output,
@Mensaje varchar(500) output
)
as
begin
	SET @Resultado = 1
	IF NOT EXISTS (SELECT * FROM CATEGORIA WHERE Pais =@Pais and IdCategoria != @IdCategoria)
		update CATEGORIA set
		Pais = @Pais,
		Estado = @Estado
		where IdCategoria = @IdCategoria
	ELSE
	begin
		SET @Resultado = 0
		set @Mensaje = 'No se puede repetir la descripcion de una categoria'
	end

end

go

create procedure sp_EliminarCategoria(
@IdCategoria int,
@Resultado bit output,
@Mensaje varchar(500) output
)
as
begin
	SET @Resultado = 1
	IF NOT EXISTS (
	 select *  from CATEGORIA c
	 inner join MONERA p on p.IdCategoria = c.IdCategoria
	 where c.IdCategoria = @IdCategoria
	)
	begin
	 delete top(1) from CATEGORIA where IdCategoria = @IdCategoria
	end
	ELSE
	begin
		SET @Resultado = 0
		set @Mensaje = 'La categoria se encuentara relacionada a un Monera'
	end

end

GO

------------------------------------------------------------------------------------------------------------------------------------------------------


insert into NEGOCIO(IdNegocio,Nombre,RFC,Direccion, Telefono, Correo) values
(1,'Sistema Ventas','XAXX010101000','','','')
select * from NEGOCIO

select* from categoria


insert into PERMISO(IdRol,NombreMenu, Estado) values 
(1, 'MENUUSARIO', 1)


INSERT INTO ROL(Descripcion)
VALUES('PROGRAMADOR')

DELETE FROM ROL
WHERE IdRol= 1 ;

INSERT INTO PERMISO(IdRol,NombreMenu)VALUES

(1,'MENUUSARIO'),
(1,'MENUMANTE'),
(1,'MENUVEN'),
(1,'MENUCOM'),
(1,'MENICLIENT'),
(1,'MENUPROVE'),
(1,'MENURE'),
(1,'MENUACER')

INSERT INTO PERMISO(IdRol,NombreMenu)VALUES
(2,'MENUVEN'),
(2,'MENUCOM'),
(2,'MENICLIENT'),
(2,'MENUPROVE'),
(2,'MENUACER')
-----------------------------------------------------------------------MONERA----------------------------------------------------------------------------

drop procedure sp_RegistrarMonera

	Create PROC sp_RegistrarMonera(
	@Codigo varchar(20),
	@Nombre varchar(30),
	@Descripcion Varchar(30),
	@IdCategoria int,
	@Estado bit,
	@Resultado INT output,
	@Mensaje varchar(500) output
	)
	as
	begin
		 set @Resultado = 0;

		 if not exists(select * from MONERA WHERE Codigo = @Codigo)
		 begin 
			  insert into MONERA(Codigo, Nombre, Descripcion, IdCategoria ,Estado)VALUES
			  (@Codigo,@Nombre,@Descripcion,@IdCategoria,@Estado)
			  set @Resultado=SCOPE_IDENTITY()
		 end
		 else
			 set @Mensaje = 'Ya existe un MONERA con el mismo codigo';
	end
	go


Create PROC sp_EditarMonera(
@IdMonera int,
@Codigo varchar(20),
@Nombre varchar(30),
@Descripcion Varchar(30),
@IdCategoria int,
@Estado bit,
@Resultado INT output,
@Mensaje varchar(500) output
)
as
begin
     set @Resultado = 1

     if not exists(select * from MONERA WHERE Codigo = @Codigo and IdMonera !=@IdMonera)
    
          update MONERA set
		  Codigo=@Codigo,
		  Nombre= @Nombre,
		  Descripcion=@Descripcion,
		  IdCategoria=@IdCategoria,
		  Estado=@Estado
		  where  IdMonera=@IdMonera
    
     else
	 begin
	     set @Resultado=0
         set @Mensaje = 'Ya existe un MONERA con el mismo codigo';
	 end
end

go

CREATE PROC sp_EliminarMonera (
    @IdMonera int,
    @Respuesta bit output,
	@Mensaje varchar(500) output
)
AS
BEGIN
    SET @Respuesta = 1
    SET @Mensaje = ''
    declare @pasoreglas bit=1

    IF  EXISTS (SELECT * FROM DETALLE_COMPRA dc
	inner join MONERA p on p.IdMonera=dc.IdMonera
	where p.IdMonera=@IdMonera)
    begin
	     set @pasoreglas=0
		 set @Respuesta=0
		 set @Mensaje=@Mensaje + 'No se puede eliminar por que se encuentra relacionado a una COMPRA\n'
	end

	IF  EXISTS (SELECT * FROM DETALLE_VENTA dv
	inner join MONERA p on p.IdMonera=dv.IdMonera
	where p.IdMonera=@IdMonera)
	begin
	   set @pasoreglas=0
	   set @Respuesta=0
	   set @Mensaje=@Mensaje + 'No se puede eliminar por que se encuentra relacionado a una VENTA\n'
	end
	IF(@pasoreglas=1)
	begin
	     delete from MONERA WHERE IdMonera=@IdMonera
		 set @Respuesta=1
	end
end


go



--------------------------------------------------CLIENTE-------------------------------------------------------------------------------------------------------------

create PROC sp_RegistrarCliente(
@Documento varchar(50),
@NombreCompleto varchar(50),
@Correo varchar(50),
@Telefono varchar(50),
@Estado bit,
@Resultado int output,
@Mensaje varchar(500) output
)as
begin
	SET @Resultado = 0
	DECLARE @IDPERSONA INT 
	IF NOT EXISTS (SELECT * FROM CLIENTE WHERE Documento = @Documento)
	begin
		insert into CLIENTE(Documento,NombreCompleto,Correo,Telefono,Estado) values (
		@Documento,@NombreCompleto,@Correo,@Telefono,@Estado)

		set @Resultado = SCOPE_IDENTITY()
	end
	else
		set @Mensaje = 'El cliente ya existe'
end

go

create PROC sp_ModificarCliente(
@IdCliente int,
@Documento varchar(50),
@NombreCompleto varchar(50),
@Correo varchar(50),
@Telefono varchar(50),
@Estado bit,
@Resultado bit output,
@Mensaje varchar(500) output
)as
begin
	SET @Resultado = 1
	DECLARE @IDPERSONA INT 
	IF NOT EXISTS (SELECT * FROM CLIENTE WHERE Documento = @Documento and IdCliente != @IdCliente)
	begin
		update CLIENTE set
		Documento = @Documento,
		NombreCompleto = @NombreCompleto,
		Correo = @Correo,
		Telefono = @Telefono,
		Estado = @Estado
		where IdCliente = @IdCliente
	end
	else
	begin
		SET @Resultado = 0
		set @Mensaje = 'El cliente ya existe'
	end
end

GO

-----------------------------------------------------------------------PROVEEDOR---------------------------------------------------------------------------------

create PROC sp_RegistrarProveedor(
@Documento varchar(50),
@Banco varchar(50),
@Correo varchar(50),
@Telefono varchar(50),
@Estado bit,
@Resultado int output,
@Mensaje varchar(500) output
)as
begin
	SET @Resultado = 0
	DECLARE @IDPERSONA INT 
	IF NOT EXISTS (SELECT * FROM PROVEEDOR WHERE Documento = @Documento)
	begin
		insert into PROVEEDOR(Documento,Banco,Correo,Telefono,Estado) values (
		@Documento,@Banco,@Correo,@Telefono,@Estado)

		set @Resultado = SCOPE_IDENTITY()
	end
	else
		set @Mensaje = 'El proveedor ya existe'
end

GO

create PROC sp_EditarProveedor(
@IdProveedor int,
@Documento varchar(50),
@Banco varchar(50),
@Correo varchar(50),
@Telefono varchar(50),
@Estado bit,
@Resultado bit output,
@Mensaje varchar(500) output
)as
begin
	SET @Resultado = 1
	DECLARE @IDPERSONA INT 
	IF NOT EXISTS (SELECT * FROM PROVEEDOR WHERE Documento = @Documento and IdProveedor != @IdProveedor)
	begin
		update PROVEEDOR set
		Documento = @Documento,
		Banco = @Banco,
		Correo = @Correo,
		Telefono = @Telefono,
		Estado = @Estado
		where IdProveedor = @IdProveedor
	end
	else
	begin
		SET @Resultado = 0
		set @Mensaje = 'El proveedor ya existe'
	end
end


go

create procedure sp_EliminarProveedor(
@IdProveedor int,
@Resultado bit output,
@Mensaje varchar(500) output
)
as
begin
	SET @Resultado = 1
	IF NOT EXISTS (
	 select *  from PROVEEDOR p
	 inner join COMPRA c on p.IdProveedor = c.IdProveedor
	 where p.IdProveedor = @IdProveedor
	)
	begin
	 delete top(1) from PROVEEDOR where IdProveedor = @IdProveedor
	end
	ELSE
	begin
		SET @Resultado = 0
		set @Mensaje = 'No se puede eliminar, el proveedor se encuentara relacionado a una compra'
	end

end

go


----------------------------------------------------------------COMPRA-----------------------------------------------------------------------------------------

CREATE TYPE [dbo].[EDetalle_Compra] AS TABLE(
	[IdMonera] int NULL,
	[PrecioCompra] decimal(18,2) NULL,
	[PrecioVenta] decimal(18,2) NULL,
	[Cantidad] int NULL,
	[MontoTotal] decimal(18,2) NULL
)

GO

CREATE PROCEDURE sp_RegistrarComprass(
    @IdUsuario int,
    @IdProveedor int,
    @IdCliente int,
    @TipoDocumento varchar(500),
    @NumeroDocumento varchar(500),
    @MontoTotal decimal(18,2),
    @DetalleCompra [EDetalle_Compra] READONLY,
    @Resultado bit output,
    @Mensaje varchar(500) output
)
AS
BEGIN
    DECLARE @IdUsuarioDebug int;
    SET @IdUsuarioDebug = @IdUsuario;
    PRINT 'IdUsuario: ' + CAST(@IdUsuarioDebug AS VARCHAR);

    BEGIN TRY
        DECLARE @idcompra int = 0;
        SET @Resultado = 1;
        SET @Mensaje = '';

        IF NOT EXISTS (SELECT 1 FROM USUARIO WHERE IdUsuario = @IdUsuario)
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'El IdUsuario especificado no existe en la tabla USUARIO.';
            RETURN;
        END

        BEGIN TRANSACTION registro;

        IF @IdProveedor IS NOT NULL
        BEGIN
            INSERT INTO COMPRA (IdUsuario, IdProveedor, TipoDocumento, NumeroDocumento, MontoTotal)
            VALUES (@IdUsuario, @IdProveedor, @TipoDocumento, @NumeroDocumento, @MontoTotal);
        END

        ELSE IF @IdCliente IS NOT NULL
        BEGIN
            INSERT INTO COMPRA (IdUsuario, IdCliente, TipoDocumento, NumeroDocumento, MontoTotal)
            VALUES (@IdUsuario, @IdCliente, @TipoDocumento, @NumeroDocumento, @MontoTotal);
        END

        SET @idcompra = SCOPE_IDENTITY();

        INSERT INTO DETALLE_COMPRA (IdCompra, IdMonera, PrecioCompra, PrecioVenta, Cantidad, MontoTotal)
        SELECT @idcompra, IdMonera, PrecioCompra, PrecioVenta, Cantidad, MontoTotal FROM @DetalleCompra;

        UPDATE p
        SET p.Stock = p.Stock + dc.Cantidad,
            p.PrecioCompra = dc.PrecioCompra,
            p.PrecioVenta = dc.PrecioVenta
        FROM MONERA p
        INNER JOIN @DetalleCompra dc ON dc.IdMonera = p.IdMonera;

        COMMIT TRANSACTION registro;
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION registro;
    END CATCH;
END;

-----------------------------------------------VENTA----------------------------------------------------------------

CREATE TYPE [dbo].[EDetalle_Venta] AS TABLE( 
	[IdMonera] int NULL,
	[PrecioVenta] decimal(18,2) NULL,
	[Cantidad] int NULL,
	[SubTotal] decimal(18,2) NULL
)


GO


create procedure sp_RegistrarVenta(
@IdUsuario int,
@TipoDocumento varchar(500),
@NumeroDocumento varchar(500),
@DocumentoCliente varchar(500),
@NombreCliente varchar(500),
@MontoPago decimal(18,2),
@MontoCambio decimal(18,2),
@MontoTotal decimal(18,2),
@DetalleVenta [EDetalle_Venta] READONLY,                                      
@Resultado bit output,
@Mensaje varchar(500) output
)
as
begin
	
	begin try

		declare @idventa int = 0
		set @Resultado = 1
		set @Mensaje = ''

		begin  transaction registro

		insert into VENTA(IdUsuario,TipoDocumento,NumeroDocumento,DocumentoCliente,NombreCliente,MontoPago,MontoCambio,MontoTotal)
		values(@IdUsuario,@TipoDocumento,@NumeroDocumento,@DocumentoCliente,@NombreCliente,@MontoPago,@MontoCambio,@MontoTotal)

		set @idventa = SCOPE_IDENTITY()

		insert into DETALLE_VENTA(IdVenta,IdMonera,PrecioVenta,Cantidad,SubTotal)
		select @idventa,IdMonera,PrecioVenta,Cantidad,SubTotal from @DetalleVenta

		commit transaction registro

	end try
	begin catch
		set @Resultado = 0
		set @Mensaje = ERROR_MESSAGE()
		rollback transaction registro
	end catch

end

go

select*from VENTA


----------------------------------------------------------REPORTE--------------------------------------


create PROC sp_ReporteCompras(
 @fechainicio varchar(10),
 @fechafin varchar(10),
 @idproveedor int
 )
  as
 begin

  SET DATEFORMAT dmy;
   select 
 convert(char(10),c.FechaRegistro,103)[FechaRegistro],c.TipoDocumento,c.NumeroDocumento,c.MontoTotal,
 u.NombreCompleto[UsuarioRegistro],
 pr.Documento[DocumentoProveedor],pr.Banco,
 p.Codigo[CodigoProducto],p.Nombre[NombreProducto],ca.Pais[Categoria],dc.PrecioCompra,dc.PrecioVenta,dc.Cantidad,dc.MontoTotal[SubTotal]
 from COMPRA c
 inner join USUARIO u on u.IdUsuario = c.IdUsuario
 inner join PROVEEDOR pr on pr.IdProveedor = c.IdProveedor
 inner join DETALLE_COMPRA dc on dc.IdCompra = c.IdCompra
 inner join MONERA p on p.IdMonera = dc.IdMonera
 inner join CATEGORIA ca on ca.IdCategoria = p.IdCategoria
 where CONVERT(date,c.FechaRegistro) between @fechainicio and @fechafin
 and pr.IdProveedor = iif(@idproveedor=0,pr.IdProveedor,@idproveedor)
 end

 go
 EXEC sp_ReporteCompras '01/01/2023', '12/31/2023', 00001;




 CREATE PROC sp_ReporteVentas(
 @fechainicio varchar(10),
 @fechafin varchar(10)
 )
 as
 begin
 SET DATEFORMAT dmy;  
 select 
 convert(char(10),v.FechaRegistro,103)[FechaRegistro],v.TipoDocumento,v.NumeroDocumento,v.MontoTotal,
 u.NombreCompleto[UsuarioRegistro],
 v.DocumentoCliente,v.NombreCliente,
 p.Codigo[CodigoProducto],p.Nombre[NombreProducto],ca.Pais[Pais],dv.PrecioVenta,dv.Cantidad,dv.SubTotal
 from VENTA v
 inner join USUARIO u on u.IdUsuario = v.IdUsuario
 inner join DETALLE_VENTA dv on dv.IdVenta = v.IdVenta
 inner join MONERA p on p.IdMonera = dv.IdMonera
 inner join CATEGORIA ca on ca.IdCategoria = p.IdCategoria
 where CONVERT(date,v.FechaRegistro) between @fechainicio and @fechafin
end

EXEC sp_ReporteVentas '2023-01-01', '2023-12-31';

