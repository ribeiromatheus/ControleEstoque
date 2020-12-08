var salvar_customizado = null;

function formatar_mensagem_aviso(mensagens) {
    //var ret = '';

    //for (var i = 0; i < mensagens.length; i++) {
    //    ret += '<li>' + mensagens[i] + '</li>';
    //}

    //return '<ul>' + ret + '</ul>';

    const template =
        '<ul>' +
        '{{ #. }}' +
        '<li>{{ . }}</li>' +
        '{{ /. }}' +
        '</ul> ';

    return Mustache.render(template, mensagens);
}

function marcar_ordenacao_campo(coluna) {
    let ordem_crescente = true,
        ordem = $(coluna).find('i');

    if (ordem.length > 0) {
        ordem_crescente = ordem.hasClass('glyphicon-arrow-down');
        if (ordem_crescente) {
            ordem.removeClass('glyphicon-arrow-down');
            ordem.addClass('glyphicon glyphicon-arrow-up');
        }
        else {
            ordem.addClass('glyphicon-arrow-up');
            ordem.removeClass('glyphicon-arrow-down');
        }
    }
    else {
        $('.coluna-ordenacao i').remove();
        $(coluna).append('&nbsp;<i class="glyphicon glyphicon-arrow-down" style="color: #000"></i>');
    }
}

function obter_ordem_grid() {
    let colunas_grid = $('.coluna-ordenacao'),
        ret = '';

    colunas_grid.each((index, item) => {
        let coluna = $(item),
            ordem = coluna.find('i');

        if (ordem.length > 0) {
            ordem_crescente = ordem.hasClass('glyphicon-arrow-down');
            ret = coluna.attr('data-campo') + (ordem_crescente ? '' : ' desc');
            return true;
        }
    });
    return ret;
}

function abrir_form(dados) {
    set_dados_form(dados);

    let modal_cadastro = $('#modal_cadastro');

    $('#msg_mensagem_aviso').empty();
    $('#msg_aviso').hide();
    $('#msg_mensagem_aviso').hide();
    $('#msg_erro').hide();

    bootbox.dialog({
        title: "Cadastro de " + tituloPagina,
        message: modal_cadastro,
        className: 'dialogo',
    })
        .on('shown.bs.modal', () => {
            modal_cadastro.show(0, () => {
                set_focus_form();
            });
        })
        .on('hidden.bs.modal', () => {
            modal_cadastro.hide().appendTo('body');
        });
}

function criar_linha_grid(dados) {
    const template = $('#template-grid').html();

    return Mustache.render(template, dados);

    //var ret =
    //    '<tr data-id=' + dados.Id + '>' +
    //    set_dados_grid(dados) +
    //    '<td>' +
    //    '<a class="btn btn-primary btn-alterar" role="button" style="margin-right: 3px"><i class="glyphicon glyphicon-pencil"></i> Alterar</a>' +
    //    '<a class="btn btn-danger btn-excluir" role="button"><i class="glyphicon glyphicon-trash"></i> Excluir</a>' +
    //    '</td>' +
    //    '</tr>';

    //return ret;
}

function salvar_ok(response, param) {
    if (response.Resultado == "OK") {
        if (param.Id == 0) {
            param.Id = response.IdSalvo;
            $('#grid_cadastro').removeClass('invisivel');
            $('#mensagem_grid').addClass('invisivel');
            $('#quantidade_registros').text(response.Quantidade)

            const btn = $('ul.pagination > li.active').first();
            const pagina = (btn && btn.length == 1) ? parseInt(btn.text()) : 1;
            atualizar_grid(pagina);
        }
        else {
            let linha = $('#grid_cadastro').find('tr[data-id=' + param.Id + ']').find('td');
            preencher_linha_grid(param, linha);
        }

        $('#modal_cadastro').parents('.bootbox').modal('hide');
    }
    else if (response.Resultado == "ERRO") {
        $('#msg_aviso').hide();
        $('#msg_mensagem_aviso').hide();
        $('#msg_erro').show();
    }
    else if (response.Resultado == "AVISO") {
        $('#msg_mensagem_aviso').html(formatar_mensagem_aviso(response.Mensagens));
        $('#msg_aviso').show();
        $('#msg_mensagem_aviso').show();
        $('#msg_erro').hide();
    }
}

function salvar_erro() {
    swal('Aviso', 'Não foi possível salvar. Tente novamente em instantes.', 'warning');
}

function atualizar_grid(pagina, btn) {
    var ordem = obter_ordem_grid(),
        filtro = $('#txt_filtro'),
        tamPag = $('#ddl_tam_pag').val(),
        url = url_page_click,
        param = { 'pagina': pagina, 'tamPag': tamPag, 'filtro': filtro.val(), 'ordem': ordem };

    $.post(url, add_anti_forgery_token(param), (response) => {
        if (response) {
            let table = $('#grid_cadastro').find('tbody');

            table.empty();
            if (response.length > 0) {
                $('#grid_cadastro').removeClass('invisivel');
                $('#mensagem_grid').addClass('invisivel');
                for (let i = 0; i < response.length; i++) {
                    table.append(criar_linha_grid(response[i]));
                }
            }
            else {
                $('#grid_cadastro').addClass('invisivel');
                $('#mensagem_grid').removeClass('invisivel');
            }

            if (btn) {
                btn.siblings().removeClass('active');
                btn.addClass('active');
            }
        }
    })
        .fail(() => {
            swal('Aviso', 'Não foi possível recuperar as informações. Tente novamente em instantes.', 'warning');
        });
}

$(document).on('click', '#btn_incluir', () => {
    abrir_form(get_dados_inclusao());
})
    .on('click', '.btn-alterar', function () {
        let btn = $(this),
            id = btn.closest('tr').attr('data-id'),
            url = url_alterar,
            param = { 'id': id };

        $.post(url, add_anti_forgery_token(param), (response) => {
            if (response) {
                abrir_form(response);
            }
        })
            .fail(() => {
                swal('Aviso', 'Não foi possível recuperar as informações. Tente novamente em instantes.', 'warning');
            });
    })
    .on('click', '.btn-excluir', function () {
        let btn = $(this),
            tr = btn.closest('tr'),
            id = tr.attr('data-id'),
            url = url_excluir,
            param = { 'id': id };

        bootbox.confirm({
            message: "Realmente deseja excluir o " + tituloPagina + "?",
            buttons: {
                confirm: {
                    label: 'Sim',
                    className: 'btn-danger'
                },
                cancel: {
                    label: 'Não',
                    className: 'btn-success'
                }
            },
            callback: (result) => {
                if (result) {
                    $.post(url, add_anti_forgery_token(param), (response) => {
                        if (response.Ok) {
                            tr.remove();
                            let quant = $('#grid_cadastro > tbody > tr').length;
                            if (quant == 0) {
                                $('#grid_cadastro').addClass('invisivel');
                                $('#mensagem_grid').removeClass('invisivel');
                            }

                            $('#quantidade_registros').text(response.Quantidade)

                        }
                    })
                        .fail(() => {
                            swal('Aviso', 'Não foi possível excluir. Tente novamente em instantes.', 'warning');
                        });
                }
            }
        });
    })
    .on('click', '#btn_confirmar', function () {
        let btn = $(this),
            url = url_confirmar,
            param = get_dados_form();

        if (salvar_customizado && typeof (salvar_customizado) == "function") {
            salvar_customizado(url, param, salvar_ok, salvar_erro)
        }
        else {
            $.post(url, add_anti_forgery_token(param), (response) => {
                salvar_ok(response, param);
            })
                .fail(() => {
                    salvar_erro();
                });
        }
    })
    .on('click', '.page-item', function () {
        const btn = $(this);
        const pagina = parseInt(btn.text());
        atualizar_grid(pagina, btn);
    })
    .on('change', '#ddl_tam_pag', function () {
        let ordem = obter_ordem_grid(),
            ddl = $(this),
            tamPag = ddl.val(),
            filtro = $('#txt_filtro'),
            pagina = 1,
            url = url_tam_pag_change,
            param = { 'pagina': pagina, 'tamPag': tamPag, 'filtro': filtro.val(), 'ordem': ordem };

        $.post(url, add_anti_forgery_token(param), (response) => {
            if (response) {
                var table = $('#grid_cadastro').find('tbody');

                table.empty();
                if (response.length > 0) {
                    $('#grid_cadastro').removeClass('invisivel');
                    $('#mensagem_grid').addClass('invisivel');
                    for (let i = 0; i < response.length; i++) {
                        table.append(criar_linha_grid(response[i]));
                    }
                }
                else {
                    $('#grid_cadastro').addClass('invisivel');
                    $('#mensagem_grid').removeClass('invisivel');
                }
            }
        })
            .fail(() => {
                swal('Aviso', 'Não foi possível recuperar as informações. Tente novamente em instantes.', 'warning');
            });
    })
    .on('keyup', '#txt_filtro', function () {
        let ordem = obter_ordem_grid(),
            filtro = $(this),
            ddl = $('#ddl_tam_pag'),
            tamPag = ddl.val(),
            pagina = 1,
            url = url_filtro_change,
            param = { 'pagina': pagina, 'tamPag': tamPag, 'filtro': filtro.val(), 'ordem': ordem };

        $.post(url, add_anti_forgery_token(param), (response) => {
            if (response) {
                let table = $('#grid_cadastro').find('tbody');

                table.empty();
                if (response.length > 0) {
                    $('#grid_cadastro').removeClass('invisivel');
                    $('#mensagem_grid').addClass('invisivel');

                    for (let i = 0; i < response.length; i++) {
                        table.append(criar_linha_grid(response[i]));
                    }
                }
                else {
                    $('#grid_cadastro').addClass('invisivel');
                    $('#mensagem_grid').removeClass('invisivel');
                }
            }
        })
            .fail(() => {
                swal('Aviso', 'Não foi possível recuperar as informações. Tente novamente em instantes.', 'warning');
            });
    })
    .on('click', '.coluna-ordenacao', function () {
        marcar_ordenacao_campo($(this));

        var ordem = obter_ordem_grid(),
            filtro = $('#txt_filtro'),
            ddl = $('#ddl_tam_pag'),
            tamPag = ddl.val(),
            pagina = 1,
            url = url_filtro_change,
            param = { 'pagina': pagina, 'tamPag': tamPag, 'filtro': filtro.val(), 'ordem': ordem };

        $.post(url, add_anti_forgery_token(param), (response) => {
            if (response) {
                let table = $('#grid_cadastro').find('tbody');

                table.empty();
                if (response.length > 0) {
                    $('#grid_cadastro').removeClass('invisivel');
                    $('#mensagem_grid').addClass('invisivel');

                    for (let i = 0; i < response.length; i++) {
                        table.append(criar_linha_grid(response[i]));
                    }
                }
                else {
                    $('#grid_cadastro').addClass('invisivel');
                    $('#mensagem_grid').removeClass('invisivel');
                }
            }
        })
            .fail(() => {
                swal('Aviso', 'Não foi possível recuperar as informações. Tente novamente em instantes.', 'warning');
            });
    })
    .ready(() => {
        var grid = $('#grid_cadastro > tbody');
        for (let i = 0; i < linhas.length; i++) {
            grid.append(criar_linha_grid(linhas[i]));
        }

        marcar_ordenacao_campo($('#grid_cadastro thead tr th:nth-child(1) span'));
    });