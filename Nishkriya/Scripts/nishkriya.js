$().ready(function() {
    $('.spoilerbox').addClass('spoilerText');
    $('.spoilerbox').on('click', function () {
         $(this).removeClass('spoilerText');
    });
});