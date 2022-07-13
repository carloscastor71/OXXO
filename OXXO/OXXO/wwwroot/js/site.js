// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.



let Checked = null;
//The class name can vary
for (let CheckBox of document.getElementsByClassName('only-one')) {
    CheckBox.onclick = function () {
        if (Checked != null) {
            Checked.checked = false;
            Checked = CheckBox;
        }
        Checked = CheckBox;
    }
}

$('#filtrar').click(function (e) {

    let Id;
    var data = [];
    var search = [];
    const $elemento = document.querySelector("#chips");
    $elemento.innerHTML = "";

    e.preventDefault();
    var frm = $('#dataForm');
    data = frm.serializeArray();

    for (var i = 0; i < data.length; i++) {

        if (data[i].value != '') {
            $("#chips").append("<li>" + data[i].name + ": " + data[i].value + "</li>");
            search[i] = { "name": data[i].name, "value": data[i].value };
        } else {
            search[i] = { "name": data[i].name, "value": "NULL" }
        }
    }

    $.ajax({
        url: '/MesaControl/Buscar',
        data: search,
        type: 'POST',
        dataType: "json",
        bFilter: false,
        success: function (json) {
            $('#categorizacion').DataTable({
                data: json[0].data,
                paging: true,
                destroy: true,
                scrollX: true,
                searching: true,
                language: {
                    "lengthMenu": "Mostrar _MENU_ registros",
                    "zeroRecords": "No se encontró ningun registro",
                    "info": "Mostrando _PAGE_ página(s) de _PAGES_",
                    "infoEmpty": "No hay registros encontrados",
                    "infoFiltered": "(filtrado de _MAX_ registros)",
                    "search": "Buscar",
                    "serchPlaceholder": "Buscar",
                    "paginate": {
                        "previous": "Anterior",
                        "next": "Siguiente"
                    },

                },
                columns: [
                    { data: 'IdEmisor' },
                    { data: 'RFC' },
                    { data: 'NombreCompleto' },
                    { data: 'NombreComercial' },
                    { data: 'Direccion' },
                    { data: 'Banco' },
                    { data: 'Estatus' },
                    { data: 'EmailConfirmado' },
                    {
                        render: function (data, type, full, meta) {
                            return '<a href="/MesaControl/Editar?RFC=' + full.RFC + '" class="btn btn-outline-secundary btn-sm" style="border-radius: 0px; "><i class="bi bi-pencil-square"></i></a>';
                        }
                    },
                    {
                        render: function (data, type, full, meta) {
                            return '<a href="/CargaDocumentos/Index?RFC=' + full.RFC + '" class="btn btn-outline-secundary btn-sm" style="border-radius: 0px; "><i class="bi bi-folder-plus"></i></a>';
                        }
                    },
                    {
                        render: function (data, type, full, meta) {
                            return '<a href="/MesaControl/Verificacion?RFC=' + full.RFC + '" class="btn btn-outline-secundary btn-sm" style="border-radius: 0px; "><i class="bi bi-clipboard-check"></i></a>';
                        }
                    },
                    {
                        render: function (data, type, full, meta) {
                            return '<a class="btn btn-outline-secundary btn-sm" style="border-radius: 0px;" data-bs-toggle="modal" data-bs-target="#sendDoc" data-bs-whatever=' + full.RFC + ' data-bs-whatever2=' + full.Correo + '><i class="bi bi-envelope-plus"></i></a>';
                        }
                    },
                    {
                        render: function (data, type, full, meta) {

                            return '<a id="editCat" data-url="Categorizar" type="submit" class="btn btn-outline-secundary btn-sm" style="border-radius: 0px;" data-bs-toggle="offcanvas" data-bs-target="#offcanvasRightEdit" data-bs-whatever=' + full.RFC + ' ><i class="bi bi-tags-fill"></i></a>';
                        }
                    },

                ],
                columnDefs: [
                    { "className": "dt-center", "targets": "_all" },
                    {
                        targets: 6,

                        render: function (data, type, row, meta) {

                            if (data == "Pendiente") {

                                return '<input type="button" class="btn btn-warning btn-sm disabled-button" value="' + data + '"/>';
                            }
                            if (data == "Aprobado") {

                                return '<input type="button" class="btn btn-success btn-sm disabled-button" value="' + data + '"/>';
                            }
                            if (data == "Rechazado") {

                                return '<input type="button" class="btn btn-danger btn-sm disabled-button" value="' + data + '"/>';
                            }

                        }
                    },
                    {
                        targets: 7,

                        render: function (data, type, row, meta) {

                            if (data == "1") {


                                return '<input type="button" class="btn btn-warning btn-sm disabled-button" value="Pendiente"/>';
                            }
                            if (data == "2") {

                                return '<input type="button" class="btn btn-success btn-sm disabled-button" value="Aprobado"/>';
                            }
                            if (data == "3") {

                                return '<input type="button" class="btn btn-danger btn-sm disabled-button" value="Rechazado"/>';
                            }

                        }
                    },

                    {

                        targets: 8,
                        visible: json[0].permiso === false ? false : true,
                        render: function (data, type, full, meta) {
                            return '<a href="/MesaControl/Editar?RFC=' + full.RFC + '" class="btn btn-outline-secundary btn-sm" style="border-radius: 0px; "><i class="bi bi-pencil-square"></i></a>';

                        }

                    },
                    {

                        targets: 9,
                        visible: json[0].permiso === false ? false : true,
                        render: function (data, type, full, meta) {
                            return '<a href="/CargaDocumentos/Index?RFC=' + full.RFC + '" class="btn btn-outline-secundary btn-sm" style="border-radius: 0px; "><i class="bi bi-folder-plus"></i></a>';
                        }

                    },
                    {

                        targets: 10,
                        visible: json[0].permiso === false ? false : true,
                        render: function (data, type, full, meta) {
                            return '<a href="/MesaControl/Verificacion?RFC=' + full.RFC + '" class="btn btn-outline-secundary btn-sm" style="border-radius: 0px; "><i class="bi bi-clipboard-check"></i></a>';
                        }

                    },
                    {

                        targets: 11,
                        visible: json[0].permiso === false ? false : true,
                        render: function (data, type, full, meta) {
                            return '<a class="btn btn-outline-secundary btn-sm" style="border-radius: 0px;" data-bs-toggle="modal" data-bs-target="#sendDoc" data-bs-whatever=' + full.RFC + ' data-bs-whatever2=' + full.Correo + '><i class="bi bi-envelope-plus"></i></a>';
                        }

                    },
                    {
                        targets: 12,
                        visible: json[0].permiso === false ? false : true,
                        render: function (data, type, full, meta) {

                            return '<a id="editCat" data-url="Categorizar" type="submit" class="btn btn-outline-secundary btn-sm" style="border-radius: 0px;" data-bs-toggle="offcanvas" data-bs-target="#offcanvasRightEdit" data-bs-whatever=' + full.RFC + ' ><i class="bi bi-tags-fill"></i></a>';
                        }

                    },
                ],



            });

            //Enviar Documento por email obtener valores de correo y rfc por button
            var exampleModal = document.getElementById('sendDoc')
            exampleModal.addEventListener('show.bs.modal', function (event) {

                var button = event.relatedTarget

                var recipient = button.getAttribute('data-bs-whatever')
                var fullcorreo = button.getAttribute('data-bs-whatever2')

                var modalTitle = exampleModal.querySelector('.modal-title')
                var modalBodyInput = exampleModal.querySelector('.modal-body input')
                var modalLabel = exampleModal.querySelector('.modal-label')

                modalTitle.textContent = 'Enviar documento a: ' + recipient
                modalBodyInput.value = fullcorreo
                modalLabel.value = recipient
                modalLabel.textContent = recipient


            })

            //obtiene valor del boton categorizar
            var offcanvas = document.getElementById('offcanvasRightEdit')
            offcanvas.addEventListener('show.bs.offcanvas', function (event) {
                var button = event.relatedTarget

                Id = button.getAttribute('data-bs-whatever')
            })
        },
        error: function () {
            Swal.fire({
                title: json[0].mensaje,
                text: json[0].data,
                icon: 'error',
                confirmButtonText: 'cerrar'
            });
        }

    });
});
