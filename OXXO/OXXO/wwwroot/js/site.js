﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
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
            search[i] = { "name" : data[i].name, "value": data[i].value }; 
        }else{
            search[i] = { "name": data[i].name , "value" : "NULL" }
        }
    }

    $.ajax({
        url: '/MesaControl/Buscar',
        data: search,
        type: 'POST',
        bFilter: false,
        success: function (json) {
            $('#categorizacion').DataTable({
                data: json.data,
                paging: false,
                destroy: true,
                searching: false,
                columns: [
                    { data: 'idEmisor' },
                    { data: 'rfc' },
                    { data: 'nombreCompleto' },
                    { data: 'nombreComercial' },
                    { data: 'direccion' },
                    { data: 'banco' },
                    { data: 'estatus' },
                    {
                        render: function (data, type, full, meta) {
                            return '<a href="/CargaDocumentos/Index?RFC=' + full.rfc + '" class="btn btn-outline-secundary" style="width: 120px; border-radius: 0px; "><i class="bi bi-folder-plus"></i></a>';
                        }
                    },
                    {
                        render: function (data, type, full, meta) {
                            return '<a href="/MesaControl/Verificacion?RFC=' + full.rfc + '" class="btn btn-outline-secundary" style="width: 120px; border-radius: 0px; "><i class="bi bi-clipboard-check"></i></a>';
                        }
                    },

                    
                ],
            });
        }
    });
    $('#CreateUser').click(function (event) {
        event.preventDefault();
        event.stopImmediatePropagation();

        $.ajax({
            url: '/Usuarios/Crear/',
            type: 'GET',
            success: function (res) {
                $('#offcanvasRightCrear').html(res);
                $('#createBodyCrear').modal('show');
            },
            error: function () {
                alert("¡Hay un problema con el servicio, favor de reiniciar!")
            }
        });
    });


    

});

   
