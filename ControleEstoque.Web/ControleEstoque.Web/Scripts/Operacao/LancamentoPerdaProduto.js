function preencher_tela() {
    let id_inventario = $('#ddl_inventario').val(),
        url = url_recuperar_lista_produto_diferenca_inventario + id_inventario,
        grid = $('#grid').find('tbody');

    grid.empty();

    $.ajax({
        type: 'GET',
        processData: false,
        url: url,
        dataType: 'json',
        success: (response) => {
            if (response) {
                for (let i = 0; i < response.length; i++) {
                    grid.append(criar_linha_grid(response[i]));
                }
            }
            else {
                swal('Aviso', 'Não foi possível recuperar os produtos do invetário.', 'warning');
            }
        },
        error: () => {
            swal('Aviso', 'Não foi possível recuperar os produtos do invetário.', 'warning');
        }
    });
}

function criar_linha_grid(dados) {
    let template = $('#template-grid').html();
    return Mustache.render(template, dados);
}


function obter_dados() {
    let ret = [];

    $('#grid tbody tr').each((index, item) => {
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
    let ret = false;

    $('#grid tbody tr input[type=text]').each((index, item) => {
        if ($(item).val() != '') {
            ret = true;
            return false
        }
    });
    return ret;
}

$(document).ready(() => {
    preencher_tela();
})
    .on('change', '#ddl_inventario', () => {
        preencher_tela();
    })
    .on('click', '#btn_salvar', () => {
        if (!verificar_preenchimento()) {
            swal('Aviso', 'Para salvar, você deve preencher algum motivo.', 'warning');
        }
        else {
            let url = url_salvar,
                dados = { dados: obter_dados() };

            $.ajax({
                type: 'POST',
                processData: false,
                contentType: 'application/json',
                data: JSON.stringify(add_anti_forgery_token(dados)),
                url: url,
                dataType: 'json',
                success: (response) => {
                    if (response) {
                        swal('Aviso', 'Lançamento de perdas salvo com sucesso.', 'info');
                    }
                    else {
                        swal('Aviso', 'Não foi possível salvar os lançamentos de perdas de produtos.', 'warning');
                    }
                },
                error: () => {
                    swal('Aviso', 'Não foi possível salvar os lançamentos de perdas de produtos.', 'warning');
                }
            });
        }
    })