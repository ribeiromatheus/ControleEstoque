var sequencia = 1;

function formatar_data(data) {
    let dia = ('0' + data.getDate()).slice(-2),
        mes = ('0' + (data.getMonth() + 1)).slice(-2);
    return data.getFullYear() + "-" + mes + "-" + dia;
}

function incluir_linha_produto() {
    $('#grid tbody').append(Mustache.render($('#template-produto').html(), { Sequencia: sequencia }));
    sequencia++;
    atualiza_quantidade_estoque('select[id^="ddl_produto_"]');
}

function limpar_tela() {
    $('#txt_numero').val('');
    $('#grid tbody').empty();
    incluir_linha_produto();

    let ddl = $('#grid tbody tr select:first');
    if (ddl.length > 0) {
        ddl.trigger('change');
    }
}

function obter_lista_saidas() {
    let ret = []

    $('#grid tbody tr').each(function (index, item) {
        var txt_quantidade = $(this).find('input'),
            ddl_produto = $(this).find('select'),
            quantidade = parseInt(txt_quantidade.val()),
            produto = parseInt(ddl_produto.val());

        if (quantidade > 0) {
            ret.push({ IdProduto: produto, Quantidade: quantidade });
        }
    });

    return ret;
}

function atualiza_quantidade_estoque(this_select) {
    let url = url_recuperar_quantidade_estoque_produto,
        ddl = $(this_select),
        div_quant_estoque = ddl.closest('tr').find('td[class="quant-estoque"]'),
        dados = {
            id: parseInt(ddl.val())
        };

    $.post(url, add_anti_forgery_token(dados), (response) => {
        if (response.OK) {
            div_quant_estoque.text(response.Result);
        }
    })
        .fail(() => {
            swal('Aviso', 'Não foi possível obter a quantidade em estoque do produto.', 'warning');
        });
}

$(document).ready(() => {
    let hoje = new Date();
    $('#txt_data').val(formatar_data(hoje));

    limpar_tela();

})
    .on('click', '#btn_incluir', () => {
        incluir_linha_produto();
    })
    .on('click', '#btn_salvar', function () {
        let btn = $(this),
            lista_saidas = obter_lista_saidas();

        if (lista_saidas.length == 0) {
            swal('Aviso', 'Para salvar, você deve informar produtos com quantidades', 'warning');
        }
        else {
            let url = url_salvar,
                dados = {
                    data: $('#txt_data').val(),
                    produtos: JSON.stringify(lista_saidas)
                };

            $.post(url, add_anti_forgery_token(dados), (response) => {
                if (response.OK) {
                    $('#txt_numero').val(response.Numero);
                    swal('Sucesso', 'Saída de produtos salva com sucesso', 'info');
                }
            })
                .fail(() => {
                    swal('Aviso', 'Não foi possível salvar a saída de produtos', 'warning');
                });
        }
    })
    .on('click', '#btn_cancelar', () => {
        let lista_saidas = obter_lista_saidas();

        if (lista_saidas.length == 0 || $('#txt_numero').val() != "") {
            limpar_tela();
        }
        else {
            swal({
                text: 'Deseja realmente cancelar a saida dos produtos?',
                type: 'info',
                showCancelButton: true,
                allowEscapeKey: false,
                allowOutsideClick: false,
                cancelButtonText: 'Não',
                confirmButtonClass: 'btn-primary',
                confirmButtonText: 'Sim'
            }).then((opcao) => {
                if (opcao.value) {
                    limpar_tela();
                }
            });
        }
    })
    .on('click', '.btn_remover', function () {
        let linha = $(this).closest('tr');
        linha.remove();
    })
    .on('change', 'select[id^="ddl_produto_"]', function () {
        atualiza_quantidade_estoque($(this));
    });