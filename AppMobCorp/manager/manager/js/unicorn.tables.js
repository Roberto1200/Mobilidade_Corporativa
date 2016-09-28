/**
 * Unicorn Admin Template
 * Version 2.0.1
 * Diablo9983 -> diablo9983@gmail.com
**/

$(document).ready(function(){
	
	var table=$('.data-table').dataTable({
	    "bJQueryUI": true,
	    "language": {
	        "emptyTable": "Nenhum registro disponível",
	        "info": "Exibindo _START_ até _END_ de _TOTAL_ registro(s)",
	        "infoEmpty": "Exibindo 0 de 0 em 0 registros",
	        "infoFiltered": "(Filtro de _MAX_ de registros)",
	        "infoPostFix": "",
	        "thousands": ",",
	        "lengthMenu": "Exibir _MENU_ registros",
	        "loadingRecords": "Processando...",
	        "processing": "Processando...",
	        "search": "Pesquisar por: ",
	        "zeroRecords": "Nenhum registro encontrado",
            
	        "paginate": {
	            "previous": "Anterior",
	            "next": "Pr&#243;xima",
	            "first": "Primeira",
	            "last": "&#218;ltima"
	        }
	    },
		"sPaginationType": "full_numbers",
		"sDom": '<""l>t<"F"fp>'
	});
	
	var checkboxClass = 'icheckbox_flat-blue';
	var radioClass = 'iradio_flat-blue';
	$('input[type=checkbox],input[type=radio]').iCheck({
    	checkboxClass: checkboxClass,
    	radioClass: radioClass
	});
	
	//$('select').select2();
	

	$("span.icon input:checkbox, th input:checkbox").on('ifChecked || ifUnchecked',function() {
		var checkedStatus = this.checked;
		var checkbox = $(this).parents('.widget-box').find('tr td:first-child input:checkbox');		
		checkbox.each(function() {
			this.checked = checkedStatus;
			if (checkedStatus == this.checked) {
				$(this).closest('.' + checkboxClass).removeClass('checked');
			}
			if (this.checked) {
				$(this).closest('.' + checkboxClass).addClass('checked');
			}
		});
	});


	//var table = $('.data-table').DataTable();

	//$('.data-table tbody').on('click', 'tr', function () {
	    
	//    if ($(this).hasClass('selected')) {
	//        $(this).removeClass('selected');
	//    }
	//    else {
	//        table.$('tr.selected').removeClass('selected');
	//        $(this).addClass('selected');
	//    }
	//});

	//$('#button').click(function () {
	//    table.row('.selected').remove().draw(false);
	//});

});
