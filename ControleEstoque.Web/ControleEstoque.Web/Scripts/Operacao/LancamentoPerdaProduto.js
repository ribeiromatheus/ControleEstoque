function preencher_tela() {
    var id_inventario = $('#ddl_inventario').val(),
        url = url_recuperar_lista_produto_diferenca_inventario + id_inventario,
        grid = $('#grid').find('tbody');

    grid.empty();

    $.ajax({
        type: 'GET',
        processData: false,
        url: url,
        dataType: 'json',
        success: function (response) {
            if (response) {
                for (var i = 0; i < response.length; i++) {
                    grid.append(criar_linha_grid(response[i]));
                }
            }
            else {
                swal('Aviso', 'Não foi possível recuperar os produtos do invetário.', 'warning');
            }
        },
        error: function () {
            swal('Aviso', 'Não foi possível recuperar os produtos do invetário.', 'warning');
        }
    });
}

function criar_linha_grid(dados) {
    var template = $('#template-grid').html();
    return Mustache.render(template, dados);
}


function obter_dados() {
    var ret = [];

    $('#grid tbody tr').each(function (index, item) {
        var tr = $(item),
            id = tr.attr('data-id'),
            motivo = tr.find('input[type=text]').val();

        if (motivo != '') {
            ret.push({ Id: id, Motivo: motivo });
        }
    });
    return ret;
}

function verificar_preenchimento() {
    var ret = false;

    $('#grid tbody tr input[type=text]').each(function (index, item) {
        if ($(item).val() != '') {
            ret = true;
            return false
        }
    });
    return ret;
}

$(document).ready(function () {
    preencher_tela();
})
    .on('change', '#ddl_inventario', function () {
        preencher_tela();
    })
    .on('click', '#btn_salvar', function () {
        if (!verificar_preenchimento()) {
            swal('Aviso', 'Para salvar, você deve preencher algum motivo.', 'warning');
        }
        else {
            var url = url_salvar,
                dados = { dados: obter_dados() };

            $.ajax({
                type: 'POST',
                processData: false,
                contentType: 'application/json',
                data: JSON.stringify(add_anti_forgery_token(dados)),
                url: url,
                dataType: 'json',
                success: function (response) {
                    if (response) {
                        swal('Aviso', 'Lançamento de perdas salvo com sucesso.', 'info');
                    }
                    else {
                        swal('Aviso', 'Não foi possível salvar os lançamentos de perdas de produtos.', 'warning');
                    }
                },
                error: function () {
                    swal('Aviso', 'Não foi possível salvar os lançamentos de perdas de produtos.', 'warning');
                }
            });
        }
    })