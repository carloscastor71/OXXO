// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    /*$('#example').DataTable({
        "lengthChange": false,
        "bInfo": false,
        "searching": false,
        language: {
            "decimal": "",
            "emptyTable": "No hay información",
            "infoFiltered": "(Filtrado de _MAX_ total entradas)",
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "search": "Buscar:",
            "zeroRecords": "Sin resultados encontrados",
            "paginate": {
                "first": "Primero",
                "last": "Ultimo",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        }
    });*/
   
});

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
        url: '/Categorizacion/Buscar',
        data: search,
        type: 'POST',
        success: function (json) {

            $(document).ready(function () {
                $('#categorizacion').DataTable({
                    data: json.data,
                    destroy: true,
                    columns: [
                        { data: 'idEmisor' },
                        { data: 'nombreCompleto' },
                        { data: 'rfc' },
                        { data: 'direccion' },
                        { data: 'banco' },
                        { data: 'estatus' },
                    ],
                });
            });
        }
    });

});