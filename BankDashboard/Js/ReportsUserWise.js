(function ($) {

    $(document).ready(function () {
        //alert("report user wise document ready");
        
        LoadDatesInModal();
       // BindDropDown();
        $('#ddlStatusList').change(function () {
           
            var stsText = $('#ddlStatusList option:selected').text();
           // alert(stsText);
            $('#status').val(stsText);
          //  alert($('#status').val());
        });
      

        $('#userReportType').change(function () {
            
            var stsTextType = $('#userReportType option:selected').text();
            // alert(stsText);
            //if (stsTextType == 'Maximum Dispute Raising Machine') {
            //    $('#SearchUserType').attr('action', 'GetMaxDisputedMachine');
            //}
            //else if (stsTextType == 'Maximum Complaint Raising Customer')
            //{
            //    $('#SearchUserType').attr('action', 'GetMaxDisputedCustomer');
            //}
            //else
            //{
            //    $('#SearchUserType').attr('action', 'ReportsUserWise');
            //}
            if (stsTextType == 'User Wise Transactions') {
                $('#ddlUserList').prop('selectedIndex', 0);
                $('#ddlUserList1').css('display', 'block');           
            }
            else {
                $('#ddlUserList1').css('display', 'none');
            }

            $('#reportType').val(stsTextType);
            if (stsTextType == 'Maximum Complaint Raising Customer' || stsTextType=='Maximum Dispute Raising Machine' ) {
                $("#txtTerminalId").attr("disabled", "disabled");
                $("#ddlStatusList").attr("disabled", "disabled");
            }
            else {
                $("#txtTerminalId").removeAttr("disabled");
                $("#ddlStatusList").removeAttr("disabled");
            }
        });
    });
    function LoadDatesInModal() {
        //alert("report user wise document ready");
        // $(".invoice").datepicker('setdate', new Date());
        $(".invoice").datepicker(
            {
                dateFormat: 'dd-M-yy',
                timepicker: false
            }
        );
    }
   
})($);

