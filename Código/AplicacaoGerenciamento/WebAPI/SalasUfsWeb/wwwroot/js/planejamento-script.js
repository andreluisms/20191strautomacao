﻿$(document).ready(function () {
    if (document.querySelector('#mensagem-retorno'))
        document.getElementById("mensagem-retorno").click();

    if (document.querySelector('.horarios-planejamento'))
        document.getElementById("btn-create-planejamento").disabled = false;
    else 
        document.getElementById("btn-create-planejamento").disabled = true;

});

function AdicionarNovoHorario() {

    let horarioInicio = $('#horarioInicio').val();
    let horarioFim = $('#horarioFim').val();
    let dia = $('#input-dia-semana').val();
    let indice = 0;
    

    if (horarioInicio.length > 0 && horarioFim.length > 0 && dia.length > 0) {

        var novoHorario = new Array();
        novoHorario.push(adicionaHorarioNaTabela(indice, horarioInicio, horarioFim, dia));

        let horarios = document.getElementsByClassName('horarios-planejamento');
        if (document.querySelector('.horarios-planejamento')) {
            for (indice = 0; indice < horarios.length; indice++) {

                dia = horarios[indice].childNodes[0].childNodes[2].value;
                horarioInicio = horarios[indice].childNodes[0].childNodes[0].value;
                horarioFim = horarios[indice].childNodes[0].childNodes[1].value;

                novoHorario.push(adicionaHorarioNaTabela(indice + 1, horarioInicio, horarioFim, dia));
            }

        } 

        document.getElementById('container-horarios').innerHTML = "";
        for (var i = 0; i < novoHorario.length; i++)
            $('#container-horarios').append(novoHorario[i]);

        document.getElementById("btn-create-planejamento").disabled = false;
    }
}

function adicionaHorarioNaTabela(indice, horarioInicio, horarioFim, dia){
    let idItem = 'novo-horario-' + indice;
    let horario =
        '<tr id="' + idItem + '" class="horarios-planejamento">' +
        '<td>' +
            '<input class="form-control" type="time" name="Horarios[' + indice + '].HorarioInicio" hidden readonly  value = "' + horarioInicio + '"/>' +
            '<input class="form-control" type="time" name="Horarios[' + indice + '].HorarioFim" hidden readonly  value = "' + horarioFim + '"/>' +
            '<input class="form-control" name="Horarios[' + indice + '].DiaSemana" hidden readonly value = "' + dia + '"/>' +
            '<p class="form-control">' + horarioInicio + ' / ' + horarioFim  + ' - '+ dia +'</p>' + 
        '</td>' +
        
        '<td>' +
            '<a id="remove-novo-horario"  onclick="RemoveNovoHorario(' + '\''+idItem + '\''+ ')" class="btn btn-danger"><i class="nav-icon fa fa-trash text-white"></i> </a>' +
        '</td>' +
        '</tr>';

    return horario;
}


function RemoveNovoHorario(idItem) {

    document.getElementById(idItem).remove();

    let horarios = document.getElementsByClassName('horarios-planejamento');

    var novosHorarios = new Array();
    for (var indice = 0; indice < horarios.length; indice++)
        novosHorarios.push(adicionaHorarioNaTabela(indice,
            horarios[indice].childNodes[0].childNodes[0].value,
            horarios[indice].childNodes[0].childNodes[1].value,
            horarios[indice].childNodes[0].childNodes[2].value));

    document.getElementById('container-horarios').innerHTML = "";
    for (var indice = 0; indice < novosHorarios.length; indice++) 
        $('#container-horarios').append(novosHorarios[indice]);

    if (!document.querySelector('.horarios-planejamento'))
        document.getElementById("btn-create-planejamento").disabled = true;     
} 