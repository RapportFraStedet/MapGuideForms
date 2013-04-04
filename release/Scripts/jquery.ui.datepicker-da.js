/* Danish initialisation for the jQuery UI date picker plugin. */
/* Written by Jan Christensen ( deletestuff@gmail.com). */
jQuery(function($){
    $.datepicker.regional['da'] = {
		closeText: 'Luk',
        prevText: '&#x3c;Forrige',
		nextText: 'N&#230;ste&#x3e;',
		currentText: 'Idag',
        monthNames: ['Januar','Februar','Marts','April','Maj','Juni',
        'Juli','August','September','Oktober','November','December'],
        monthNamesShort: ['Jan','Feb','Mar','Apr','Maj','Jun',
        'Jul','Aug','Sep','Okt','Nov','Dec'],
		dayNames: ['S&#248;ndag','Mandag','Tirsdag','Onsdag','Torsdag','Fredag','L&#248;rdag'],
		dayNamesShort: ['S&#248;n','Man','Tir','Ons','Tor','Fre','L&#248;r'],
		dayNamesMin: ['S&#248;','Ma','Ti','On','To','Fr','L&#248;'],
		weekHeader: 'Uge',
        dateFormat: 'dd-mm-yy',
		firstDay: 1,
		isRTL: false,
		showMonthAfterYear: false,
		yearSuffix: ''};
    $.datepicker.setDefaults($.datepicker.regional['da']);
});