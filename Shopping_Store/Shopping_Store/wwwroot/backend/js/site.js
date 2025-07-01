// site.js
$(function () { // <-- Bao bọc toàn bộ code này
    // ... code cho a.confirmDeletion
    if ($("a.confirmDeletion").length) {
        $("a.confirmDeletion").click(() => {
            if (!confirm("Confirm deletion")) return false;
        });
    }

    // ... code cho div.alert.notification
    if ($("div.alert.notification").length) {
        setTimeout(() => {
            $("div.alert.notification").fadeOut();
        }, 2000);
    }

    // ... code cho Product.ImgUpload (nếu bạn chuyển nó vào site.js)
    $("#Product_ImgUpload").change(function () {
        readURL(this);
    });

    // ... code CKEditor (nếu bạn chuyển nó vào site.js)
    CKEDITOR.replace('Product_Description');

}); // <-- Kết thúc $(function() {})
// Hàm readURL nếu nó là một hàm độc lập
function readURL(input) {
    // ...
}