function set_dados_form(dados) {
    $('#id_cadastro').val(dados.Id);
    $('#txt_nome').val(dados.Nome);
    $('#rbtn_pessoa_juridica').prop('checked', false);
    $('#rbtn_pessoa_fisica').prop('checked', false);
    $('#txt_num_documento').val(dados.NumDocumento);
    $('#txt_razao_social').val(dados.RazaoSocial);
    $('#txt_telefone').val(dados.Telefone);
    $('#txt_contato').val(dados.Contato);
    $('#txt_logradouro').val(dados.Logradouro);
    $('#txt_numero').val(dados.Numero);
    $('#txt_complemento').val(dados.Complemento);
    $('#txt_cep').val(dados.Cep);
    $('#cbx_ativo').prop('checked', dados.Ativo);

    if (dados.Tipo == 2) {
        $('#rbtn_pessoa_juridica').prop('checked', true).trigger('click');
    }
    else {
        $('#rbtn_pessoa_fisica').prop('checked', true).trigger('click');
    }

    var inclusao = (dados.Id == 0);
    if (inclusao) {
        reset_dropdown_list(true, true);
    }
    else {
        $('#ddl_pais').val(dados.IdPais);
        mudar_pais(dados.IdEstado, dados.IdCidade);
    }
}

function reset_dropdown_list(ddl_estado_status, ddl_cidade_status) {
    $('#ddl_estado').empty();
    $('#ddl_estado').prop('disabled', ddl_estado_status);

    $('#ddl_cidade').empty();
    $('#ddl_cidade').prop('disabled', ddl_cidade_status);
}

function mudar_pais(id_estado, id_cidade) {
    let ddl_pais = $('#ddl_pais'),
        id_pais = parseInt(ddl_pais.val()),
        ddl_estado = ('#ddl_estado'),
        ddl_cidade = $('#ddl_cidade');

    if (id_pais > 0) {
        var url = url_listar_estados,
            param = { idPais: id_pais };

        reset_dropdown_list(true, true);

        $.post(url, add_anti_forgery_token(param), (response) => {
            if (response && response.length > 0) {
                for (let i = 0; i < response.length; i++) {
                    $(ddl_estado).append('<option value=' + response[i].Id + '>' + response[i].Nome + '</option>')
                }
                $(ddl_estado).prop('disabled', false);
                $(ddl_estado).find('option:eq(0)').prop('selected', true);
            }
        });
        sel_estado(id_estado);
        mudar_estado(id_cidade);
    }
    else {
        reset_dropdown_list(true, true);
    }
}

function mudar_estado(id_cidade) {
    var ddl_estado = $('#ddl_estado'),
        id_estado = parseInt(ddl_estado.val()),
        ddl_cidade = ('#ddl_cidade');

    if (id_estado > 0) {
        let url = url_listar_cidades,
            param = { idEstado: id_estado };

        $(ddl_cidade).empty()
        $(ddl_cidade).prop('disabled', true)

        $.post(url, add_anti_forgery_token(param), (response) => {
            if (response && response.length > 0) {
                for (let i = 0; i < response.length; i++) {
                    $(ddl_cidade).append('<option value=' + response[i].Id + '>' + response[i].Nome + '</option>')
                }
                $(ddl_cidade).prop('disabled', false);
                $(ddl_cidade).find('option:eq(0)').prop('selected', true);
            }
        });
        sel_cidade(id_cidade);
    } else {
        reset_dropdown_list(false, true);
    }
}

function sel_estado(id_estado) {
    $('#ddl_estado').val(id_estado);
    $('#ddl_estado').prop('disabled', $('#ddl_estado option').length == 0);
}

function sel_cidade(id_cidade) {
    $('#ddl_cidade').val(id_cidade);
    $('#ddl_cidade').prop('disabled', $('#ddl_cidade option').length == 0);
}

function set_focus_form() {
    $('#txt_nome').focus();
}

//function set_dados_grid(dados) {
//    return '<td>' + dados.Nome + '</td>' +
//        '<td>' + (dados.Ativo ? 'SIM' : 'NÃO') + '</td>'
//}

function get_dados_inclusao() {
    return {
        Id: 0,
        Nome: '',
        Tipo: 2,
        NumDocumento: '',
        RazaoSocial: '',
        Telefone: '',
        Contato: '',
        Logradouro: '',
        Numero: '',
        Complemento: '',
        Cep: '',
        IdPais: 0,
        IdEstado: 0,
        IdCidade: 0,
        Ativo: true
    };
}

function get_dados_form() {
    return {
        Id: $('#id_cadastro').val(),
        Nome: $('#txt_nome').val(),
        Tipo: $('#rbtn_pessoa_juridica').is(':checked') ? 2 : 1,
        NumDocumento: $('#txt_num_documento').val(),
        RazaoSocial: $('#txt_razao_social').val(),
        Telefone: $('#txt_telefone').val(),
        Contato: $('#txt_contato').val(),
        Logradouro: $('#txt_logradouro').val(),
        Numero: $('#txt_numero').val(),
        Complemento: $('#txt_complemento').val(),
        Cep: $('#txt_cep').val(),
        IdPais: $('#ddl_pais').val(),
        IdEstado: $('#ddl_estado').val(),
        IdCidade: $('#ddl_cidade').val(),
        Ativo: $('#cbx_ativo').prop('checked')
    };
}

function preencher_linha_grid(param, linha) {
    linha
        .eq(0).html(param.Nome).end()
        .eq(1).html(param.Telefone).end()
        .eq(2).html(param.Contato).end()
        .eq(3).html(param.Ativo ? 'SIM' : 'NÃO');
}

$(document)
    .ready(() => {
        $('#txt_telefone').mask('(00) 0000-0000');
        $('#txt_cep').mask('00000-000');
    })
    .on('click', '#rbtn_pessoa_juridica', () => {
        $('label[for="txt_num_documento"]').text('CNPJ');
        $('#txt_num_documento').mask('00.000.000/0000-00', { reverse: true });
        $('#container_razao_social').removeClass('invisible');
    })
    .on('click', '#rbtn_pessoa_fisica', () => {
        $('label[for="txt_num_documento"]').text('CPF');
        $('#txt_num_documento').mask('000.000.000-00', { reverse: true });
        $('#container_razao_social').addClass('invisible');
    })
    .on('change', '#ddl_pais', () => {
        mudar_pais();
    })
    .on('change', '#ddl_estado', () => {
        mudar_estado();
    })
    .on('blur', '#txt_cep', () => {
        let txt_cep = $('#txt_cep').val(),
            url = url_busca_cep,
            param = { 'cep': txt_cep };

        if (!txt_cep) {
            return;
        }
        else {
            $.ajax({
                type: "POST",
                data: add_anti_forgery_token(param),
                url: url,
                dataType: "json",
                beforeSend: () => {
                    swal({
                        text: 'Buscando CEP...',
                        type: 'info',
                        allowEscapeKey: false,
                        allowOutsideClick: false,
                        onBeforeOpen: () => {
                            swal.showLoading();
                        }
                    });
                },
                success: (response) => {
                    $('#txt_logradouro').val(response.Logradouro);
                    swal.close();
                },
                error: () => {
                    swal('Aviso', 'Não foi possível encontrar o CEP.', 'warning');
                }
            });
        }
    });