// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('#example').DataTable({
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
    });
   
});

$('#filtrar').click(function (e) {
    var data = [];
    const $elemento = document.querySelector("#chips");
    $elemento.innerHTML = "";


    e.preventDefault();
    var frm = $('#dataForm');
    data = frm.serializeArray();

    for (var i = 0; i < data.length; i++) {

        if (data[i].value != '') {
            $("#chips").append("<li>" + data[i].name + ": " + data[i].value + "</li>");
        }
    }

});