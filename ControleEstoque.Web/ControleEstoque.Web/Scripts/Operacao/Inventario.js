function limpar_tela() {
    $('#grid tbody tr input').each((index, input) => {
        $(input).val('');
    });
}

function verificar_preenchimento() {
    let ret = false;

    $('#grid tbody tr input[type=number]').each((index, input) => {
        if ($(input).val() != '') {
            ret = true;
            return false;
        }
    });

    return ret;
}

function obter_dados_inventario() {
    let ret = [];

    $('#grid tbody tr').each((index, item) => {
        let tr = $(item),
            quant_estoque = parseInt(tr.find('td').eq(4).text()),
            id_produto = tr.attr('data-id'),
            quant_inventario = parseInt(tr.find('input[type=number]').val()),
            motivo = tr.find('input[type=text]').val();
        if (quant_inventario > 0) {
            ret.push({
                IdProduto: id_produto,
                QuantidadeEstoque: quant_estoque,
                QuantidadeInventario: quant_inventario,
                Motivo: motivo
            });
        }
    });
    return ret;
}

$(document).ready(() => {
    limpar_tela();
})
    .on('click', '#btn_incluir', () => {
        if (verificar_preenchimento()) {
            swal({
                html: 'Algumas quantidades já foram preenchidas,<br>Deseja realmente limpar os dados para iniciar um novo inventário?',
                type: 'info',
                showCancelButton: true,
                allowEscapeKey: false,
                allowOutsideClick: false,
                cancelButtonText: 'Não',
                confirmButtonClass: 'btn btn-primary',
                confirmButtonText: 'Sim'
            }).then((opcao) => {
                if (opcao.value) {
                    limpar_tela();
                }
            });
        }
        else {
            limpar_tela();
        }
    })
    .on('click', '#btn_salvar', () => {
        if (!verificar_preenchimento()) {
            swal('Aviso', 'Para salvar, você deve preencher todas as quantidades.', 'warning');
        }
        else {
            let url = url_salvar,
                dados = { dados: obter_dados_inventario() };

            $.ajax({
                type: 'POST',
                processData: false,
                contentType: 'application/json',
                data: JSON.stringify(add_anti_forgery_token(dados)),
                url: url,
                dataType: 'json',
                success: (response) => {
                    if (response.OK) {
                        swal('Sucesso', 'Inventário salvo com sucesso.', 'info');
                        limpar_tela();
                    }
                    else {
                        swal('Aviso', 'Não foi possível salvar o inventário.', 'warning');
                    }
                },
                error: () => {
                    swal('Aviso', 'Não foi possível salvar o inventário.', 'warning');
                }
            })
        }
    });