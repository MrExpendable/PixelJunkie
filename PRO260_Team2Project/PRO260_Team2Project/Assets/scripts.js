$(document).ready(function() 
{
    $('#simple-menu').sidr();
    
    $('#simple-menu').focus();
    
    $('.imageFilter').click(function()
    {
        $('.filter').slideToggle('fast');
    });
});