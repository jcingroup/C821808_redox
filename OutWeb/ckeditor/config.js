/**
 * @license Copyright (c) 2003-2015, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {
    // Define changes to default configuration here. For example:
    config.skin = 'bootstrapck';
    config.height = 500;
    config.language = 'zh';
    // config.uiColor = '#AADC6E';
    config.extraPlugins = 'youtube';
    config.contentsCss = ['../Content/css/editor.css'];
    // config.toolbar = [
    //     {
    //         name: "basicstyles",
    //         items: ["FontSize", "Bold", "Underline", "Strike", "-", "JustifyLeft", "JustifyCenter", "JustifyRight", "-", "RemoveFormat"]
    //     },
    //     { name: "colors", items: ["TextColor", "BGColor"] },
    //     { name: "paragraph", items: ["NumberedList", "BulletedList", "-", "Outdent", "Indent"] },
    //     // { name: "styles", items: ["Styles", "Format"] },
    //     { name: "styles", items: ["Styles"] },
    //     { name: "links", items: ["Link", "Unlink", "Anchor"] },
    //     { name: 'insert', items: ['Image', 'Table', 'Youtube'] },
    //     {
    //         name: "clipboard",
    //         items: ["Cut", "Copy", "PasteText", "PasteFromWord", "Undo", "Redo"]
    //     },
    //     { name: "document", items: ["Source", "-"] },
    //     { name: "tools", items: ["Maximize", "-"] },
    //     { name: "editing" }
    // ];
    config.toolbar = [
        { name: 'styles', items: [ 'Font', 'FontSize', 'Styles' ] },
        { name: 'basicstyles', items: [ 'Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-', 'CopyFormatting', 'RemoveFormat' ] },
        { name: 'colors', items: [ 'TextColor', 'BGColor' ] },
        { name: 'clipboard', items: [ 'Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo' ] },
        { name: 'editing', items: [ 'Find', 'Replace', '-', 'SelectAll', '-', 'Scayt' ] },
        '/',
        { name: 'paragraph', items: [ 'NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote', 'CreateDiv', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', '-', 'BidiLtr', 'BidiRtl', 'Language' ] },
        { name: 'links', items: [ 'Link', 'Unlink', 'Anchor' ] },
        { name: 'insert', items: [ 'Image', 'Table', 'Youtube', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak', 'Iframe' ] },
        { name: 'tools', items: [ 'Maximize', 'ShowBlocks' ] },
        { name: 'document', items: [ 'Source', '-', 'Save', 'NewPage', 'Preview', 'Print', '-', 'Templates' ] },
    ];
    config.autoUpdateElement = true;

    //不要轉換htmltag
    config.allowedContent = true;
    config.fontSize_sizes = '12px/12px;13/13px;16/16px;18/18px;20/20px;22/22px;24/24px;36/36px;48/48px;';
    config.font_names = 'Arial;Arial Black;Comic Sans MS;Courier New;Tahoma;Verdana;新細明體;細明體;標楷體;微軟正黑體';

    // 不要把空白<i>標籤移除 (for font icons)
    CKEDITOR.dtd.$removeEmpty['i'] = false;
};

CKEDITOR.stylesSet.add('default', [
    // Block Styles
    { name: '標題1', element: 'h2' },
    { name: '標題2', element: 'h3' },
    { name: '標題3', element: 'h4' },
    { name: '標題4', element: 'h5' },
    { name: '標題5', element: 'h6' },
    { name: '段落', element: 'p' },
]);