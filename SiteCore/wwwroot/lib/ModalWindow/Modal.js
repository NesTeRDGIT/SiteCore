var Modal = angular.module('Modal', []);
Modal.service('ModalService', Service);
function Service() {
    var modals = []; // array of modals on the page
    var service = {};

    service.Add = Add;
    service.Remove = Remove;
    service.Open = Open;
    service.Close = Close;
    service.Content = Content;
    service.Caption = Caption;
    service.Count = Count;
    return service;

    function Add(modal) {
        // add modal to array of active modals
        modals.push(modal);
    }

    function Count() {
       return  modals.length;
    }

    function Remove(id) {

        // remove modal from array of active modals
        var modalToRemove = _.findWhere(modals, { id: id });
        modals = _.without(modals, modalToRemove);
    }

    function Open(id) {
        // open modal specified by id
        var modal = _.findWhere(modals, { id: id });
        if (modal)
            modal.open();
    }

    function Close(id) {
        // close modal specified by id
        var modal = _.findWhere(modals, { id: id });
        modal.close();
        modal.content('');
    }

    function Content(id, content) {
        // close modal specified by id
        var modal = _.findWhere(modals, { id: id });
        modal.content(content);
    }
    function Caption(id, Caption) {
        // close modal specified by id
        var modal = _.findWhere(modals, { id: id });
        modal.caption(Caption);
    }
};
/*  <modal id="custom-modal-1">
        <div class="modal">
            <div class="modal-body">
                <div class="modal-head">
                    <h1>Caption</h1>
                    <span class="close" ng-click="closeModal('custom-modal-1');">&times;</span>
                </div>
                <div class="modal-content"></div>
            </div>
        </div>
        <div class="modal-background"></div>
    </modal>*/

Modal.directive('modal', ['ModalService', Directive]);
function Directive(ModalService) {
    return {
        link: function (scope, element, attrs) {

            // ensure id attribute exists
            if (!attrs.id) {
                console.error('modal must have an id');
                return;
            }

            // move element to bottom of page (just before </body>) so it can be displayed above everything else
            //   element.appendTo('body');
            var isPress = false;
            // close modal on background click
            element.on('mousedown', function (e) {
                var target = $(e.target);
                if (!target.closest('.modal-body').length) {
                    isPress = true;
                }
            });

            element.on('mouseup', function (e) {
                if (isPress === true) {
                    var target = $(e.target);
                    if (!target.closest('.modal-body').length) {
                        scope.$evalAsync(Close);
                    }
                }
                isPress = false;
            });
           
            var index = 1000+ModalService.Count()*5;
            element.css('z-index', index);
            element.find('div.modal-background').css('z-index', index);
            element.find('div.modal').css('z-index', index+1);
            element.find('div.modal-body').css('z-index', index + 2);

            // add self (this modal instance) to the modal service so it's accessible from controllers
            var modal = {
                id: attrs.id,
                open: Open,
                close: Close,
                content: Content,
                caption: Caption
            };

            ModalService.Add(modal);

            // remove self from modal service when directive is destroyed
            scope.$on('$destroy', function () {
                ModalService.Remove(attrs.id);
                element.remove();
            });

            // open modal
            function Open() {
                element.addClass('modal-open');
                element.show(200);
                $('body').css('overflow', 'hidden');
            }

            // close modal
            function Close() {
                element.hide();
                element.removeClass('modal-open');
                $('body').css('overflow', 'scroll');
            }

            function Content(content) {
                element.find('div.modal-content').html(content);
            }
            function Caption(Caption) {
                element.find('.modal-head').find('h1').html(Caption);
            }


        }
    };
};