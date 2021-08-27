$(document).ready(function () {
    $("input[id^='MainContent_ClaimItemList_InvDate']").datepicker({ dateFormat: "dd-mm-yy",
        onSelect: function (dateText) {
//            __doPostBack(this, dateText);
        }
    });
    $("input[id^='MainContent_ClaimItemList_ClaimDate']").datepicker({ dateFormat: "dd-mm-yy",
        onSelect: function (dateText) {
            //            __doPostBack(this, dateText);
        }
    });

    $("input[id^='MainContent_txtContactedDate']").datepicker({ dateFormat: "dd-mm-yy",
        onSelect: function (dateText) {
//                        __doPostBack(this, dateText);
        }
    });

    $("input[id^='MainContent_txtActionDate1']").datepicker({ dateFormat: "dd-mm-yy",
        onSelect: function (dateText) {
            //                        __doPostBack(this, dateText);
        }
    });
    $("input[id^='MainContent_txtActionDate2']").datepicker({ dateFormat: "dd-mm-yy",
        onSelect: function (dateText) {
            //                        __doPostBack(this, dateText);
        }
    });
    $("input[id^='MainContent_txtActionDate3']").datepicker({ dateFormat: "dd-mm-yy",
        onSelect: function (dateText) {
            //                        __doPostBack(this, dateText);
        }
    });
    $("input[id^='MainContent_txtActionDate4']").datepicker({ dateFormat: "dd-mm-yy",
        onSelect: function (dateText) {
            //                        __doPostBack(this, dateText);
        }
    });
    $("input[id^='MainContent_txtActionDate5']").datepicker({ dateFormat: "dd-mm-yy",
        onSelect: function (dateText) {
            //                        __doPostBack(this, dateText);
        }
    });

    $("input[id^='MainContent_txtFromDate']").datepicker({ dateFormat: "dd-mm-yy",
        onSelect: function (dateText) {
            //                        __doPostBack(this, dateText);
        }
    });

    $("input[id^='MainContent_txtToDate']").datepicker({ dateFormat: "dd-mm-yy",
        onSelect: function (dateText) {
            //                        __doPostBack(this, dateText);
        }
    });
});


function ValidateFile() {


    var validFilesTypes = ["bmp", "BMP", "gif", "GIF", "png", "PNG", "JPG", "jpg", "JPEG", "jpeg", "PDF", "pdf"];

    var file = document.getElementById("MainContent_PhotosUpload");

    var label = document.getElementById("MainContent_lblUploadMsg");

    var path = file.value;

    var ext = path.substring(path.lastIndexOf(".") + 1, path.length).toLowerCase();
  
    var isValidFile = false;

    for (var i = 0; i < validFilesTypes.length; i++) {

        if (ext == validFilesTypes[i]) {

            isValidFile = true;

            break;

        }

    }
   

    if (!isValidFile) {

        label.style.color = "red";

        label.innerHTML =  "Invalid File. " +

         " extension:\n\n" + validFilesTypes.join(", ");

    }
//    else if (file.files[0].size > 2097152) {
//        isValidFile = false;
//        label.style.color = "red";

//        label.innerHTML =  "Invalid File. Max file size Limit 2MB.";

//    }
    else {
        isValidFile = true;
    }

    return isValidFile;

}