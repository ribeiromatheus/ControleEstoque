var sequencia = 1;

function formatar_data(data) {
    var dia = ('0' + data.getDate()).slice(-2);
    var mes = ('0' + (data.getMonth() + 1)).slice(-2);
    return data.getFullYear() + "-" + mes + "-" + dia;
}

function incluir_linha_produto() {
    $('#grid tbody').append(Mustache.render($('#template-produto').html(), { Sequencia: sequencia }));
    sequencia++;
}

function limpar_tela() {
    $('#txt_numero').val('');
    $('#grid tbody').empty();
    incluir_linha_produto();
}

function obter_lista_entradas() {
    var ret = []

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

$(document).ready(function () {
    var hoje = new Date();
    $('#txt_data').val(formatar_data(hoje));

    limpar_tela();

})
    .on('click', '#btn_incluir', function () {
        incluir_linha_produto();
    })
    .on('click', '#btn_salvar', function () {
        var btn = $(this),
            lista_entradas = obter_lista_entradas();

        if (lista_entradas.length == 0) {
            swal('Aviso', 'Para salvar, você deve informar produtos com quantidades', 'warning');
        }
        else {
            var url = url_salvar,
                dados = {
                    data: $('#txt_data').val(),
                    produtos: JSON.stringify(lista_entradas)
                };

            $.post(url, add_anti_forgery_token(dados), function (response) {
                if (response.OK) {
                    $('#txt_numero').val(response.Numero);
                    swal('Sucesso', 'Entrada de produtos salva com sucesso', 'info');
                }
            })
                .fail(function () {
                    swal('Aviso', 'Não foi possível salvar a entrada de produtos', 'warning');
                });
        }
    })
    .on('click', '#btn_cancelar', function () {
        var lista_entradas = obter_lista_entradas();

        if (lista_entradas.length == 0 || $('#txt_numero').val() != "") {
            limpar_tela();
        }
        else {
            swal({
                text: 'Deseja realmente cancelar a entrada dos produtos?',
                type: 'info',
                showCancelButton: true,
                allowEscapeKey: false,
                allowOutsideClick: false,
                cancelButtonText: 'Não',
                confirmButtonClass: 'btn-primary',
                confirmButtonText: 'Sim'
            }).then(function (opcao) {
                if (opcao.value) {
                    limpar_tela();
                }
            });
        }
    })
    .on('click', '.btn_remover', function () {
        var linha = $(this).closest('tr');
        linha.remove();
    });