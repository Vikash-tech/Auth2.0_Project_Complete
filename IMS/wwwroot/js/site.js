// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//toastr.options = {
//    closeButton: true,
//    debug: false,
//    newestOnTop: true,
//    progressBar: true,
//    positionClass: 'toast-bottom-right',
//    preventDuplicates: false,
//    onclick: null,
//    showDuration: '300',
//    hideDuration: '1000',
//    timeOut: '5000',
//    extendedTimeOut: '1000',
//    showEasing: 'swing',
//    hideEasing: 'linear',
//    showMethod: 'fadeIn',
//    hideMethod: 'fadeOut'
//};
function ButtonsRights(btnAccvalu) {
    var userCode = btnAccvalu.sUserCode;
    var siteId = btnAccvalu.iSiteId;
    var menuId = btnAccvalu.iMenuId;

    $.ajax({
        url: '../Common/GetUserMenuAcce',
        type: 'GET',
        data: { UserCode: userCode, SiteId: siteId, MenuId: menuId },
        dataType: 'json',
        success: function (data) {

            handleButtonState(data);
        },
        error: function (error) {
            console.error('Error getting buttons right:', error);
        }
    });
}


function ButtonsRight(userCode, siteId, menuId) {
    $.ajax({
        url: '../Common/GetUserMenuAcce',
        type: 'GET',
        data: { UserCode: userCode, SiteId: siteId, MenuId: menuId },
        dataType: 'json',
        success: function (data) {

            handleButtonState(data);
        },
        error: function (error) {
            console.error('Error getting buttons right:', error);
        }
    });
}


// Place this in a separate JS file, for example, your-script.js
//function handleButtonState(data) {

//    if (data.isSetting == true) {
//        $(".ImsSetting").css("visibility", "visible");
//    }

//    if (data.isPost == true) {
//        $(".btnAdd").css("visibility", "visible");    //Save Button
//    }
//    if (data.isPut == true) {
//        $(".btnUpdate").css("visibility", "visible");  //Update Button
//    }
//    if (data.isCancel == true) {
//        $(".btnCancel").css("visibility", "visible");  //Reject Button  / Reject
//    }
//    if (data.isApprove == true) {
//        $(".btnApprove").css("visibility", "visible");  //Approve Button
//    }

//    if (data.isGet == true) {
//        $(".btnRead").css("visibility", "visible");   //View Button
//    }

//    if (data.isPMVerify == true) {
//        $(".btnPMVerify").css("visibility", "visible");   //Vierify Button
//    }
//}


function handleButtonState(data) {
    if (data.isSetting == true) {
        $(".ImsSetting").css("display", "inline-block");
    }

    if (data.isPost == true) {
        $(".btnAdd").css("display", "inline-block");    // Save Button
    }
    if (data.isPut == true) {
        $(".btnUpdate").css("display", "inline-block");  // Update Button
    }
    if (data.isCancel == true) {
        $(".btnCancel").css("display", "inline-block");  // Reject Button / Reject
    }
    if (data.isApprove == true) {
        $(".btnApprove").css("display", "inline-block"); // Approve Button
    }

    if (data.isGet == true) {
        $(".btnRead").css("display", "inline-block");   // View Button
    }

    if (data.isPMVerify == true) {
        $(".btnPMVerify").css("display", "inline-block"); // Verify Button

    }
}


function JqueryDatatable() {

    new DataTable(".JqueryTable", {

        dom: 'Bfrtip',
        fixedheader: true,
        paging: true,
        buttons: [
            {
                extend: 'excelHtml5',
                text: '<i class="fa-solid fa-file-excel"></i>',
                titleAttr: 'Excel',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'pdfHtml5',
                text: '<i class="fa-solid fa-file-pdf"></i>',
                titleAttr: 'PDF',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'print',
                text: '<i class="fa-solid fa-print"></i>',
                titleAttr: 'Print',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'colvis',
                text: '<i class="fa-solid fa-gears"></i>',
                titleAttr: 'Custom Column',
            },
        ]
    });
}