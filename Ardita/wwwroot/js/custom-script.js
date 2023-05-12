window.onload = GetClock;
$(document).ready(function () {
    textAreaCounterInit();
    textAreaCounter();
    $("select").select2({
        theme: 'bootstrap4',
        allowClear: true
    });
    $('form').on('submit', function () {
        $('html,body').scrollTop(0);
        $('body').toggleClass('sk-loading');
    });
    $('.required').each(function () {
        $(this).prop('required', true);
    });
    $('.disabled').each(function () {
        $(this).prop('disabled', true);
    });

    var menuActive = $('ul.metismenu').find('.active').children().find('.nav-label').html();
    var subMenuActive = $('ul.nav-second-level').find('.active').children().html();

    $('ol.breadcrumb').children(':first-child').next().html(menuActive);
    $('ol.breadcrumb').children(':first-child').next().next().html(subMenuActive);
    $('.page-heading').children().children('h2').html(subMenuActive);
    $('.ibox-title').children('h5').html(subMenuActive);
});

function toCapitalCase(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}

function GetClock() {
    d = new Date();
    nday = d.getDay();
    nmonth = d.getMonth() + 1;
    ndate = d.getDate();
    nyear = d.getYear();
    nhour = d.getHours();
    nmin = d.getMinutes();
    nsec = d.getSeconds();

    if (nyear < 1000) nyear = nyear + 1900;

    //if (nhour == 0) { ap = " AM"; nhour = 12; }
    //else if (nhour <= 11) { ap = " AM"; }
    //else if (nhour == 12) { ap = " PM"; }
    //else if (nhour >= 13) { ap = " PM"; nhour -= 12; }

    if (nmin <= 9) { nmin = "0" + nmin; }
    if (nsec <= 9) { nsec = "0" + nsec; }

    var clock = document.getElementById('clockbox');
    if (clock != undefined) {
        document.getElementById('clockbox').innerHTML = "" + (nhour.toString().length == 1 ? '0' + nhour : nhour) + ":" + nmin + ":" + nsec + "";
        setTimeout("GetClock()", 1000);
    }
}

function textAreaCounterInit() {
    if ($('textarea:enabled')[0]) {
        var inti = 0;
        $('textarea:enabled').each(function () {
            if ($(this).is(':visible')) {
                $(this).css('max-height', '100px');
                $(this).css('font-size', '11px');
                if (!$(this).hasClass('detail'))
                    $(this).css('height', '85px');
                else
                    $(this).css('height', '35px');
                if (!$(this).hasClass('ignore-counter') && !$(this).hasClass('note-codable')) {
                    var len = $(this).val().length;
                    var maxlength = $(this).attr('maxlength');
                    var def_maxlength = 2500;
                    $(this).css('border-radius', '5px');
                    if ($(this).hasClass('counterchar')) {

                    }
                    else if ($(this).hasClass('ledchar')) {
                        var led_maxlength = 100;
                        if (typeof maxlength !== typeof undefined && maxlength !== false) {

                        }
                        else {
                            maxlength = led_maxlength;
                            $(this).attr('maxlength', maxlength);
                        }

                        var rest = maxlength - len;
                        $(this).after('<span class="charcounterdiv" id="span-c-' + inti + '">' + rest + '/' + maxlength + '</span>');
                        $(this).attr('data-inti', inti);
                        inti++;
                    }
                    else {
                        $(this).addClass('counterchar');
                        if (typeof maxlength !== typeof undefined && maxlength !== false) { }
                        else {
                            maxlength = def_maxlength;
                            $(this).attr('maxlength', maxlength);
                        }
                        var rest = maxlength - len;
                        $(this).after('<span class="charcounterdiv" id="span-c-' + inti + '">' + rest + '/' + maxlength + '</span>');
                        $(this).attr('data-inti', inti);
                        inti++;
                    }
                }
            }
        });
    }
}

// counting input
function textAreaCounter() {
    $('.counterchar').on('input propertychange paste', function () {
        var len = $(this).val().length;
        var maxlength = $(this).attr('maxlength');
        var inti = $(this).attr('data-inti');
        $('#span-c-' + inti).text(maxlength - len + '/' + maxlength);
        if (len >= maxlength) {
            $('#span-c-' + inti).addClass('red-text');
        } else {
            $('#span-c-' + inti).removeClass('red-text');
        }
    });
}
//show message via sweatallert
function showMessage(_title, _text, _icon, _reload = false, _href = '') {
    if (_reload) {
        swal({
            title: _title,
            text: _text,
            icon: _icon,
            confirmButtonText: 'Ok'
        }).then(function () {
            if (_href != '')
                window.location.href = _href;
            else
                location.reload();
        });
    } else {
        swal({
            title: _title,
            text: _text,
            icon: _icon,
            confirmButtonText: 'Ok'
        });
    }
}
//show confirm via sweatallert
function showConfirm(_title, _text, _icon, _oktext) {
    return Swal.fire({
        title: _title,
        text: _text,
        icon: _icon,
        showCancelButton: true,
        confirmButtonText: _oktext,
    });
}
//call ajax
function ajaxcallreturn(urlstring, method, param, auth, content_type) {
    return $.ajax({
        url: urlstring,
        type: method,
        data: param,
        //contentType: content_type,
        dataType: 'json'
    });
}

function initForms(action){
    $('#btn-submit').show();
    initFormReadOnly(false);
    
    if(action == "Detail"){
        $('#btn-submit').hide();
         initFormReadOnly(true);
    }

    if(action == "Remove"){
        initFormReadOnly(true);
    }
}

function initFormReadOnly(condition){
    $("input").attr('readonly', condition);
    $("textarea").attr('readonly', condition);
    $('select').prop('disabled', condition);
}




