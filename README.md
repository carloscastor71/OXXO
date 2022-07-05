# --------OXXO--------- #
Proyecto para AGM
Desarrolladores:
       - Salvador Garcia
       - Carlos Castor
       - Miguel Salas


# Proceso GITHUB
    Usamos GitDesktop para mejor control.

    Para nuevos desarrolladores:
        1. Clonar el proyecto
        2. Crear una nueva rama a partir de Main con el nombre del desarrollador
    
    Para subir cambios:
        Situado en la rama personal del desarrollador:
        1. Hacer commit a la rama del desarrollador (Siempre poner que se módifico y si está terminado o pendiente).
        2. Push Origin a la rama del desarrollador.
        3. Crear Pull Request.
        4. Si hay algún conflicto al momento de realizar el MERGE, solucionarlo.
        5. Hacer merge.
        6. En el GitDesktop en la rama MAIN, hacer UN PULL (Repository/Pull)
            7. Cambiarse de rama a la personal y hacer un UPDATE FROM MAIN (Branch/Update From Main)
            8. En la rama personal PUSHORIGIN y listo.
            
# Estándar para las vistas
    Todo lo que escribas dentro de una nueva vista debe de estar dentro de un:
        <div class="container"></div> ----- Es un diseño especifico para que el contenido se muestre centrado.

    Y siempre debe de llevar el título cómo:
        <h1 id="titulo">NOMBRE VISTA</h1> -- Diseño especifico para fonts
    
    El filtrado o busqueda de contenido en una tabla se hará dependiendo de las especificaciones que se requieran para las tablas, pero siempre se desplegará la opción mediante un accordion lateral derecho, con un botón que tenga la leyenda (ya sea) "Buscar" o "Filtrar".


# Proceso de Funcionamiento de Menu/Controlador/Acciones:
    Para poder visualizar un nuevo controlador en el apartado de MENUS:
        EN SQL:
            1. Ir a la tabla MENU, dar de alta un nuevo apartado si así se requiere. (Este generará un ID padre)
            2. En la tabla CONTROLADOR, INSERT el submenú que deseas visualizar (con nombre, el texto que deseas que aparezca, y el Id del menú padre [PUNTO 1])
            3. En la tabla ACCIONCONTROLADOR: INSERT NOMBREACCION = 'Index', ENCABEZADO = 'Indice', ITEM = 1, IDCONTROLADOR = [Al que acabamos de insertar en el PUNTO 2]
            4. En tabla ROLCONTROLADOR: INSERT IdPerfil(El que quieras que tenga acceso a ese SUBMENÚ), IdControlador y IdAccion del submenú que insertamos en los PUNTOS anteriores, y las actividades que puede hacer (LEER,CREAR,EDITAR, poner todas en 1 si se requieren todas)
    
    Tambien existe un SP llamado SP_CrearSubmenuDefault, este sirve para hacer todo el proceso de arriba en una sola línea de código, lo unico que pide es:

        EXEC SP_CrearSubmenDefault @Nombre,@MenuPadre
            @Nombre, va a ser el nombre que le vas a asignar a ese submenu
            @MenuPadre es el ID del MENU al que va a estar asignado el submenu: 1=Registros, 2 = Transaccionalidad, 3 = Administración y 4 = Mantenimiento General

        Entonces sí mi nuevo submenú se llamara "Trabajos" y quisiera que fuera una subsección de "Administración", la ejecución del SP sería la siguiente:
            
         -----   EXEC SP_CrearSubmenuDefault 'Trabajos', 3  -----

# Comportamiento de Ventanas cómo Laterales Derechas (Para Editar, Crear o Cambiar Contraseña)
    El flujo de estas es mediante un accordion.
    VISTAS: INDEX
        - Si buscamos en la vista de Por ejemplo "TIPODOCUMENTO" en Index.cshtml, en la línea de código No. 20. Tenemos un boton que llama al apartado de "Crear Nuevo Tipo de Documento" vemos que busca el accordion para mostrar (una tipo vista modal lateral derecha).
        - Este accordion, se encuentra en la línea de código No. 139. Cómo vemos en un MODAL VACIO, solamente con propiedades de ID que usaremos para enlazar una vista parcial, en este caso "CREAR".
        - Creamos una función JS en AJAX, llamada #CreateDoc, en la línea de código No. 183. En esta llamamos a nuestro controlador y su acción para que nos traiga los datos mediante un ID. Una vez que la función nos da un SUCCESS, correctamente muestra el MODAL con la información que está en la vista parcial "CREAR.cshtml" que encontramos en la misma carpeta de VISTAS/TIPODOCUMENTO.
        - Vista Parcial (PartialView) CREAR.cshtml, Cómo vemos aquí tenemos nuestra vista Parcial pero con formato de Accordion, dónde ya mandamos nuestras funciones y datos al Controlador y Accion para poder crear un nuevo tipo de documento.
        - En el controlador principal "TipoDocumentoController.cs" solamente retornamos primero la vista parcial  con RETURN PARTIALVIEW("CREAR"). Y otro para ya crear pues ya tiene sus funciones RedirectToAction(...)

        NOTA: Se usa lo mismo para otras ventanas cómo Editar o cambiar contraseña, a excepción de algunos que filtran o son para buscar datos.


# Cómo mostrar mensajes:

    Siempre declarar "string? alert" en el metodo o actionresult.
    Despues establecerlo en: ViewBag.Alert = alert.
    Cuando se se hacerlo de la siguiente manera:
            
            ViewBag.Alert = CommonServices.ShowAlert(Alerts.Success, "Operación Correcta");
                return RedirectToAction(nameof(Index), new { alert = ViewBag.Alert });

    Utilizamos:
        Success para operaciones validadas correctamente.
        Info para mostrar algún detalle omitido.
        Warning para tener cuidado con valores utilizados anteriormente, cómo un nombre de usuario o algo especifico.
        Danger para operaciones incorrectas o de extrema atención.


# Sesiones

    Utiliza este valor para obtener alguna variable especifica que necesites usar del usuario que está actualizando o utilizando algún recurso de la aplicacion en ese momento:

                HttpContext.Session.GetString("IdUsuario"); --- Para usuario loggeado en la sesión (Digamos "fulanito de tal Rodriguez")
                HttpContext.Session.GetString("IdPerfil"); --- Para el TIPO de Usuario que está en la sesión (Administrador, Manager, Usuario Común, etc.)


# site.js
    En el archivo site.js, dentro de la carpeta wwwwroot/js/
        Se encuentra un script para el filtrado especifico con étiquetas del datatable de Mesa De Control (a tener en cuenta si se hará alguna modificación a nivel código).