/*
* File:        KeyTable.js
* Version:     1.1.1
* CVS:         jQueryIdjQuery
* Description: Keyboard navigation for HTML tables
* Author:      Allan Jardine (www.sprymedia.co.uk)
* Created:     Fri Mar 13 21:24:02 GMT 2009
* Modified:    jQueryDatejQuery by jQueryAuthorjQuery
* Language:    Javascript
* License:     GPL v2 or BSD 3 point style
* Project:     Just a little bit of fun :-)
* Contact:     www.sprymedia.co.uk/contact
* 
* Copyright 2009 Allan Jardine, all rights reserved.
*
*/

var KeyTable_sKyTuVuaNhap = "";
var KeyTable_bClicked = false;

function KeyTable(oInit) {
    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    * API parameters
    * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

    /*
    * Variable: block
    * Purpose:  Flag whether or not KeyTable events should be processed
    * Scope:    KeyTable - public
    */
    this.block = false;

    /*
    * Variable: event
    * Purpose:  Container for all event application methods
    * Scope:    KeyTable - public
    * Notes:    This object contains all the public methods for adding and removing events - these
    *           are dynamically added later on
    */
    this.event = {
        "remove": {}
    };


    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    * API methods
    * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

    /*
    * Function: fnGetCurrentPosition
    * Purpose:  Get the currently focused cell's position
    * Returns:  array int: [ x, y ]
    * Inputs:   void
    */
    this.fnGetCurrentPosition = function () {
        return [_iOldX, _iOldY];
    }


    /*
    * Function: fnGetCurrentData
    * Purpose:  Get the currently focused cell's data (innerHTML)
    * Returns:  string: - data requested
    * Inputs:   void
    */
    this.fnGetCurrentData = function () {
        if (_nOldFocus != null) {
            return _nOldFocus.innerHTML;
        }
        return null;
    }


    /*
    * Function: fnGetCurrentTD
    * Purpose:  Get the currently focused cell
    * Returns:  node: - focused element
    * Inputs:   void
    */
    this.fnGetCurrentTD = function () {
        return _nOldFocus;
    }



    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    * Private parameters
    * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

    /*
    * Variable: _nBody
    * Purpose:  Body node of the table - cached for renference
    * Scope:    KeyTable - private
    */
    var _nBody = null;
    var _tableId = oInit.table.id;
    var _nHang = Bang_nH;
    var _nCot = (_tableId == BangDuLieuID_Slide) ? Bang_nC_Slide : Bang_nC_Fixed;

    this.tableId = function () {
        return _tableId;
    }
    /*
    * Variable: 
    * Purpose:  
    * Scope:    KeyTable - private
    */
    var _nOldFocus = null;

    /*
    * Variable: _iOldX and _iOldY
    * Purpose:  X and Y coords of the old elemet that was focused on
    * Scope:    KeyTable - private
    */
    var _iOldX = null;
    var _iOldY = null;

    /*
    * Variable: _that
    * Purpose:  Scope saving for 'this' after a jQuery event
    * Scope:    KeyTable - private
    */
    var _that = null;

    /*
    * Variable: sFocusClass
    * Purpose:  Class that should be used for focusing on a cell
    * Scope:    KeyTable - private
    */
    var _sFocusClass = "focus";
    var _sFocusClass_edit = "focus_edit";

    /*
    * Variable: _bKeyCapture
    * Purpose:  Flag for should KeyTable capture key events or not
    * Scope:    KeyTable - private
    */
    var _bKeyCapture = false;

    /*
    * Variable: _oaoEvents
    * Purpose:  Event cache object, one array for each supported event for speed of searching
    * Scope:    KeyTable - private
    */
    var _oaoEvents = {
        "action": [],
        "esc": [],
        "focus": [],
        "blur": []
    };

    var _bForm;
    var _nInput;
    var _bInputFocused = false;

    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    * Private methods
    * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    * Key table events
    */

    /*
    * Function: _fnEventAddTemplate
    * Purpose:  Create a function (with closure for sKey) event addition API
    * Returns:  function: - template function
    * Inputs:   string:sKey - type of event to detect
    */
    function _fnEventAddTemplate(sKey) {
        /*
        * Function: -
        * Purpose:  API function for adding event to cache
        * Returns:  -
        * Inputs:   1. node:x - target node to add event for
        *           2. function:y - callback function to apply
        *         or
        *           1. int:x - x coord. of target cell (can be null for live events)
        *           2. int:y - y coord. of target cell (can be null for live events)
        *           3. function:z - callback function to apply
        * Notes:    This function is (interally) overloaded (in as much as javascript allows for
        *           that) - the target cell can be given by either node or coords.
        */
        return function (x, y, z) {
            if ((x == null || typeof x == "number") &&
			     (y == null || typeof y == "number") &&
			     typeof z == "function") {
                _fnEventAdd(sKey, x, y, z);
            }
            else if (typeof x == "object" && typeof y == "function") {
                var aCoords = _fnCoordsFromCell(x);
                _fnEventAdd(sKey, aCoords[0], aCoords[1], y);
            }
            else {
                alert("Unhandable event type was added: x" + x + "  y:" + y + "  z:" + z);
            }
        };
    }


    /*
    * Function: _fnEventRemoveTemplate
    * Purpose:  Create a function (with closure for sKey) event removal API
    * Returns:  function: - template function
    * Inputs:   string:sKey - type of event to detect
    */
    function _fnEventRemoveTemplate(sKey) {
        /*
        * Function: -
        * Purpose:  API function for removing event from cache
        * Returns:  int: - number of events removed
        * Inputs:   1. node:x - target node to remove event from
        *           2. function:y - callback function to apply
        *         or
        *           1. int:x - x coord. of target cell (can be null for live events)
        *           2. int:y - y coord. of target cell (can be null for live events)
        *           3. function:z - callback function to remove - optional
        * Notes:    This function is (interally) overloaded (in as much as javascript allows for
        *           that) - the target cell can be given by either node or coords and the function
        *           to remove is optional
        */
        return function (x, y, z) {
            if ((x == null || typeof arguments[0] == "number") &&
			     (y == null || typeof arguments[1] == "number")) {
                if (typeof arguments[2] == "function") {
                    _fnEventRemove(sKey, x, y, z);
                }
                else {
                    _fnEventRemove(sKey, x, y);
                }
            }
            else if (typeof arguments[0] == "object") {
                var aCoords = _fnCoordsFromCell(x);
                if (typeof arguments[1] == "function") {
                    _fnEventRemove(sKey, aCoords[0], aCoords[1], y);
                }
                else {
                    _fnEventRemove(sKey, aCoords[0], aCoords[1]);
                }
            }
            else {
                alert("Unhandable event type was removed: x" + x + "  y:" + y + "  z:" + z);
            }
        };
    }

    /* Use the template functions to add the event API functions */
    for (var sKey in _oaoEvents) {
        if (sKey) {
            this.event[sKey] = _fnEventAddTemplate(sKey);
            this.event.remove[sKey] = _fnEventRemoveTemplate(sKey);
        }
    }


    /*
    * Function: _fnEventAdd
    * Purpose:  Add an event to the internal cache
    * Returns:  -
    * Inputs:   string:sType - type of event to add, given by the available elements in _oaoEvents
    *           int:x - x-coords to add event to - can be null for "blanket" event
    *           int:y - y-coords to add event to - can be null for "blanket" event
    *           function:fn - callback function for when triggered
    */
    function _fnEventAdd(sType, x, y, fn) {
        _oaoEvents[sType].push({
            "x": x,
            "y": y,
            "fn": fn
        });
    }


    /*
    * Function: _fnEventRemove
    * Purpose:  Remove an event from the event cache
    * Returns:  int: - number of matching events removed
    * Inputs:   string:sType - type of event to look for
    *           node:nTarget - target table cell
    *           function:fn - optional - remove this function. If not given all handlers of this
    *             type will be removed
    */
    function _fnEventRemove(sType, x, y, fn) {
        var iCorrector = 0;

        for (var i = 0, iLen = _oaoEvents[sType].length; i < iLen - iCorrector; i++) {
            if (typeof fn != 'undefined') {
                if (_oaoEvents[sType][i - iCorrector].x == x &&
				     _oaoEvents[sType][i - iCorrector].y == y &&
					   _oaoEvents[sType][i - iCorrector].fn == fn) {
                    _oaoEvents[sType].splice(i - iCorrector, 1);
                    iCorrector++;
                }
            }
            else {
                if (_oaoEvents[sType][i - iCorrector].x == x &&
				     _oaoEvents[sType][i - iCorrector].y == y) {
                    _oaoEvents[sType].splice(i, 1);
                    return 1;
                }
            }
        }
        return iCorrector;
    }


    /*
    * Function: _fnEventFire
    * Purpose:  Look thought the events cache and fire off the event of interest
    * Returns:  int:iFired - number of events fired
    * Inputs:   string:sType - type of event to look for
    *           int:x - x coord of cell
    *           int:y - y coord of  ell
    * Notes:    It might be more efficient to return after the first event has been tirggered,
    *           but that would mean that only one function of a particular type can be
    *           subscribed to a particular node.
    */
    function _fnEventFire(sType, x, y) {
        //console.log( y );
        var iFired = 0;
        var aEvents = _oaoEvents[sType];
        for (var i = 0; i < aEvents.length; i++) {
            if ((aEvents[i].x == x && aEvents[i].y == y) ||
			     (aEvents[i].x == null && aEvents[i].y == y) ||
			     (aEvents[i].x == x && aEvents[i].y == null) ||
			     (aEvents[i].x == null && aEvents[i].y == null)
			) {
                aEvents[i].fn(_fnCellFromCoords(x, y), x, y);
                iFired++;
            }
        }
        return iFired;
    }

    this.fnSetViewportPosition = function (newH) {
        _fnSetViewportPosition(newH);
    }

    function _fnSetViewportPosition(newH) {
        if (Bang_Viewport_N == 0) return false;

        if (Bang_Viewport_hMin + 5 < newH && newH < Bang_Viewport_hMin + Bang_Viewport_N - 5) {
            return false;
        }

        var hMoi = newH;
        if (hMoi <= Bang_Viewport_hMin + 5) {
            hMoi = hMoi - 15;
        }
        else if (hMoi >= Bang_Viewport_hMin + Bang_Viewport_N - 5) {
            hMoi = hMoi - Bang_Viewport_N + 15;
        }

        var hMax = hMoi + Bang_Viewport_N;
        if (hMax > Bang_nH) {
            hMoi = Bang_nH - Bang_Viewport_N;
        }
        if (hMoi < 0) hMoi = 0;
        if (hMoi != Bang_Viewport_hMin) {
            Bang_Viewport_hMin = hMoi;
            Bang_HienThiDuLieu();
            _fnRefressFocusPosition();
            return true;
        }
        return false;
    }

    function _fnRefressFocusPosition() {
        if (_nOldFocus != null) {
            jQuery(_nOldFocus).removeClass(_sFocusClass);
            jQuery(_nOldFocus).removeClass(_sFocusClass_edit);
        }

        var nTarget = _fnCellFromCoords(_iOldX, _iOldY - Bang_Viewport_hMin);

        /* Add the new class to highlight the focused cell */
        if (nTarget != null) {
            if (!_bKeyCapture) {
                jQuery(nTarget).addClass(_sFocusClass);
            }
            else {
                jQuery(nTarget).addClass(_sFocusClass_edit);
            }
        }
        _nOldFocus = nTarget;
    }

    function _fnRefreshFocusClass() {
        if (_nOldFocus != null) {
            jQuery(_nOldFocus).removeClass(_sFocusClass);
            jQuery(_nOldFocus).removeClass(_sFocusClass_edit);
            if (Bang_Viewport_hMin <= _iOldY && _iOldY < Bang_Viewport_hMin + Bang_Viewport_NMax) {
                if (!_bKeyCapture) {
                    jQuery(_nOldFocus).addClass(_sFocusClass);
                }
                else {
                    jQuery(_nOldFocus).addClass(_sFocusClass_edit);
                }
            }
        }
    }

    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    * FocusXY functions
    */
    /*
    * Function: _fnSetFocusXY
    * Purpose:  Set focus on a node(x,y), and remove from an old node if needed
    * Returns:  -
    * Inputs:   node:(x,y) - node we want to focus on
    */
    function _fnSetFocusXY(x, y, valid) {
        if (typeof bValid == "undefined") {
            valid = true;
        }

        if (x == null || y == null || x < 0 || y < 0 || x >= Bang_nC || y >= Bang_nH) {
            return;
        }
        /* If node already has focus, just ignore this call */
        if (valid && _iOldX != null && _iOldY != null && _iOldX == x && _iOldY == y) {
            return;
        }

        /* Call User focus event*/
        var fnBefore = window['Bang_onBeforeFocus'];
        if (typeof fnBefore == 'function') {
            if (fnBefore(y, x) == false) {
                return;
            }
        }

        //Set viewport position
        _fnSetViewportPosition(y);

        /* Remove old focus (with blur event if needed) */
        _fnRemoveFocus(_nOldFocus);

        //Get Targe cell
        var nTarget = _fnCellFromCoords(x, y - Bang_Viewport_hMin);

        /* Cache the information that we are interested in */
        _nOldFocus = nTarget;
        _iOldX = x;
        _iOldY = y;

        /* Add the new class to highlight the focused cell */
        _fnRefreshFocusClass();

        //Khi 1 ô được chọn sẽ hiệu chỉnh của sổ để ô nằm trong vùng hiện thị
        //Lấy vị trí và kích thước ô được chọn
        var position = $(nTarget).position();
        var left0 = position.left;
        var top0 = position.top;
        var height0 = Bang_DoRongHang;
        var c_DuLieu = (_tableId == BangDuLieuID_Slide) ? AnhXaCot_Slide_DuLieu(_iOldX) : AnhXaCot_Fixed_DuLieu(_iOldX);
        var width0 = Bang_arrDoRongCot[c_DuLieu];
        //Lấy vùng hiển thị

        var div3ID = '#' + BangDuLieuID_Slide_Div;
        var div3_height = $(div3ID).height();
        var div3_width = $(div3ID).width();

        if (left0 < 0) {
            $(div3ID).scrollLeft($(div3ID).scrollLeft() + left0);
        }
        else if (left0 + width0 > div3_width) {
            $(div3ID).scrollLeft($(div3ID).scrollLeft() + left0 + width0 - div3_width + 22);
        }

        if (top0 < 0) {
            $(div3ID).scrollTop($(div3ID).scrollTop() + top0);
        }
        else if (top0 + height0 > div3_height) {
            $(div3ID).scrollTop($(div3ID).scrollTop() + top0 + height0 - div3_height + 22);
        }

        /* Fire of the focus event if there is one */
        _fnEventFire("focus", _iOldX, _iOldY);

        /* Call User focus event*/
        var fn = window['Bang_onFocus'];
        if (typeof fn == 'function') {
            fn(_iOldY, _iOldX);
        }
    }

    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    * Focus functions
    */
    /*
    * Function: _fnSetFocus
    * Purpose:  Set focus on a node, and remove from an old node if needed
    * Returns:  -
    * Inputs:   node:nTarget - node we want to focus on
    */
    function _fnSetFocus(nTarget) {
        /* Get nTarget's position  */
        if (nTarget != null) {
            var aNewPos = _fnCoordsFromCell(nTarget);
            _fnSetFocusXY(aNewPos[0], aNewPos[1] + Bang_Viewport_hMin);
        }
    }


    /*
    * Function: _fnBlur
    * Purpose:  Blur focus from the whole table
    * Returns:  -
    * Inputs:   -
    */
    function _fnBlur() {
        _fnReleaseKeys();
    }


    /*
    * Function: _fnRemoveFocus
    * Purpose:  Remove focus from a cell and fire any blur events which are attached
    * Returns:  -
    * Inputs:   node:nTarget - cell of interest
    */
    function _fnRemoveFocus(nTarget) {
        if (nTarget != null) {
            jQuery(nTarget).removeClass(_sFocusClass);
            jQuery(nTarget).removeClass(_sFocusClass_edit);
        }
        if (_iOldX != null && _iOldY != null) {
            _fnEventFire("blur", _iOldX, _iOldY);
        }
    }


    /*
    * Function: _fnClick
    * Purpose:  Focus on the element that has been clicked on by the user
    * Returns:  -
    * Inputs:   event:e - click event
    */
    function _fnClick(e) {
        if (_that.block) {
            return false;
        }

        KeyTable_bClicked = true;
        var nTarget = this;
        while (nTarget.nodeName != "TD") {
            nTarget = nTarget.parentNode;
        }

        _fnSetFocus(nTarget);
        _fnCaptureKeys();
        /* Call User focus evnet*/
        var fn = window['Bang_onClick'];
        if (typeof fn == 'function') {
            fn(_iOldX, _iOldY);
        }
    }

    /*
    * Function: _fnClick
    * Purpose:  Focus on the element that has been clicked on by the user
    * Returns:  -
    * Inputs:   event:e - click event
    */
    function _fnDblClick(e) {
        var nTarget = this;
        while (nTarget.nodeName != "TD") {
            nTarget = nTarget.parentNode;
        }
        /* Call User focus evnet*/
        var fn = window['Bang_onDblClick'];
        if (typeof fn == 'function' && _iOldX != null && _iOldY != null) {
            fn(_iOldY, _iOldX);
        }
    }


    function _fnGetNextRow(y, d, n) {
        var h = y + d, dem = 0, vR = y;
        while (h >= 0 && h < Bang_nH) {
            dem++;
            vR = h;
            if (dem >= n) {
                break;
            }
            h = h + d;
        }
        return vR;
    }

    function _fnGetNextCell(x, y, d) {
        var xR = x, yR = y, c_DuLieu;
        do {
            xR = xR + d;
            if (0 <= xR && xR < _nCot) {
                c_DuLieu = (_tableId == BangDuLieuID_Slide) ? AnhXaCot_Slide_DuLieu(xR) : AnhXaCot_Fixed_DuLieu(xR);
                if (Bang_arrHienThiCot[c_DuLieu]) {
                    return [xR, yR];
                }
            }
            else if (0 > xR) {
                xR = _nCot;
                yR = _fnGetNextRow(yR, -1, 1);
                if (y == yR) {
                    return null;
                }
            }
            else {
                xR = -1;
                yR = _fnGetNextRow(yR, 1, 1);
                if (y == yR) {
                    return null;
                }
            }
        } while (true);
        return null;
    }

    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    * Key events
    */

    /*
    * Function: _fnKey
    * Purpose:  Deal with a key events, be it moving the focus or return etc.
    * Returns:  bool: - allow browser default action
    * Inputs:   event:e - key event
    */
    function _fnKey(e) {
        /* If user or system has blocked KeyTable from doing anything, just ignore this event */
        if (_that.block || !_bKeyCapture || document.getElementById("txtONhapDuLieu")) {
            return true;
        }
        /* If a modifier key is pressed (exapct shift), ignore the event */
        if (e.metaKey || e.altKey || e.ctrlKey) {
            return true;
        }

        /* Capture shift+tab to match the left arrow key */
        var iKey = (e.keyCode == 9 && e.shiftKey) ? -1 : e.keyCode;

        var fnKeyUp = window['Bang_onCellKeyUp'];
        if (typeof fnKeyUp == 'function') {
            if (fnKeyUp(_iOldY, _iOldX, e, iKey) == false) {
                return false;
            }
        }
        var fnKeypress = window['Bang_onCellKeypress'];
        if (typeof fnKeypress == 'function') {
            if (fnKeypress(_iOldY, _iOldX, e, iKey) == false) {
                return false;
            }
        }

        var x = null, y = null;


        var iRecordPerPage = 10;
        switch (iKey) {
            case 46: /* DELETE */
                /* Call User event Keypress DELETE*/
                e.preventDefault();
                var fnDelete = window['Bang_onKeypress_Delete'];
                if (typeof fnDelete == 'function') {
                    fnDelete(_iOldY, _iOldX);
                }
                return false;

                //            case 120: /* F9 */
                //                //Focus vào ô hiện hành
                //                e.preventDefault();
                //                _fnEventFire("action", _iOldX, _iOldY);
                //                return false;

            case 9: /* tab */
            case 112: /* F1 */
            case 113: /* F2 */
            case 114: /* F3 */
            case 115: /* F4 */
            case 116: /* F5 */
            case 117: /* F6 */
            case 118: /* F7 */
            case 119: /* F8 */
            case 120: /* F9 */
            case 121: /* F10 */
            case 122: /* F11 */
            case 123: /* F12 */
                //Hủy các phím chức năng
                //e.preventDefault();
                return true;

            case 27: /* esc */
                if (!_fnEventFire("esc", _iOldX, _iOldY - Bang_Viewport_hMin)) {
                    /* Only lose focus if there isn't an escape handler on the cell */
                    //_fnBlur();
                }
//                e.preventDefault();
//                return true;
               return false;
               
            case 13: /* return(Enter) */
                //Chuyển sang ô tiếp theo
                e.preventDefault();
                if (_fnSetFocusNextCell() == false) {
                    var fnNotSetCellFocus = window['Bang_onEnter_NotSetCellFocus'];
                    if (typeof fnNotSetCellFocus == 'function') {
                        fnNotSetCellFocus();
                    }
                }
                return false;

            case 35: /* end */
                var pos35 = _fnGetNextCell(_nCot, _iOldY, -1);
                if (pos35 == null) return false;
                x = pos35[0];
                y = pos35[1];
                break;

            case 36: /* home */
                var pos36 = _fnGetNextCell(-1, _iOldY, 1);
                if (pos36 == null) return false;
                x = pos36[0];
                y = pos36[1];
                break;

            case -1:
            case 37: /* left arrow */
                var pos37 = _fnGetNextCell(_iOldX, _iOldY, -1);
                if (pos37 == null) return false;
                x = pos37[0];
                y = pos37[1];
                break;

            case 39: /* right arrow */
                var pos39 = _fnGetNextCell(_iOldX, _iOldY, 1);
                if (pos39 == null) return false;
                x = pos39[0];
                y = pos39[1];
                break;

            case 38: /* up arrow */
                x = _iOldX;
                y = _fnGetNextRow(_iOldY, -1, 1);
                break;

            case 33: /* page up */
                x = _iOldX;
                y = _fnGetNextRow(_iOldY, -1, iRecordPerPage);
                break;

            case 40: /* down arrow */
                x = _iOldX;
                y = _fnGetNextRow(_iOldY, 1, 1);
                break;

            case 34: /* page down */
                x = _iOldX;
                y = _fnGetNextRow(_iOldY, 1, iRecordPerPage);
                break;

            default: /* Nothing we are interested in */
                var MaKhoa;
                if (iKey == 0) {
                    MaKhoa = e.charCode;
                } else {
                    MaKhoa = iKey;
                }
                if (browserID == 'msie') {
                    if (96 <= MaKhoa && MaKhoa <= 105) {
                        MaKhoa = MaKhoa - 48;
                    }
                }
                else if (browserID == 'mozilla') {
                }
                else {
                }

                if ((48 <= MaKhoa && MaKhoa <= 58) ||
                    (96 <= MaKhoa && MaKhoa <= 122) ||
                    (65 <= MaKhoa && MaKhoa <= 90)) {
                    //Nhập số
                    KeyTable_sKyTuVuaNhap = String.fromCharCode(MaKhoa);
                }
                if (MaKhoa <= 122) {
                    //Nhập số
                    _fnEventFire("action", _iOldX, _iOldY - Bang_Viewport_hMin);
                }

                return true;
        }

        _fnSetFocusXY(x, y);
        return false;
    }

    function _fnSetFocusNextCell() {

        var x, y, c_DuLieu;

        x = _iOldX;
        y = _iOldY;
        while (true) {
            var pos = _fnGetNextCell(x, y, 1);
            if (pos == null) return false;
            x = pos[0];
            y = pos[1];
            c_DuLieu = (_tableId == BangDuLieuID_Slide) ? AnhXaCot_Slide_DuLieu(x) : AnhXaCot_Fixed_DuLieu(x);
            if (Bang_arrEdit[y][c_DuLieu]) {
                _fnSetFocusXY(x, y);
                return true;
            }
        }
        return false;
    }

    this.isFocus = function () {
        return _that.block == false && _bKeyCapture && document.getElementById("txtONhapDuLieu") == null;
    }

    this.focus = function () {
        _fnCaptureKeys();
    }

    this.blur = function () {
        _fnBlur();
    }

    this.fnSetFocus = function (y, x) {
        if (x != null && y != null) {
            _fnSetFocusXY(x, y, false);
        }
    }

    this.fnSetFocusNextCell = function () {
        return _fnSetFocusNextCell();
    }

    this.fnSetFocusFirstEditCell = function () {
        var x, y, c_DuLieu;

        for (y = 0; y < Bang_nH; y++) {
            for (x = 0; x < _nCot; x++) {
                c_DuLieu = (_tableId == BangDuLieuID_Slide) ? AnhXaCot_Slide_DuLieu(x) : AnhXaCot_Fixed_DuLieu(x);
                if (Bang_arrEdit[y][c_DuLieu] && Bang_arrHienThiCot[c_DuLieu]) {
                    _fnSetFocusXY(x, y);
                    return false;
                }
            }
        }
        return false;
    }

    this.Row = function () {
        return _iOldY;
    }

    this.Col = function () {
        return _iOldX + Bang_nC_Fixed;
    }

    /*
    * Function: _fnCaptureKeys
    * Purpose:  Start capturing key events for this table
    * Returns:  -
    * Inputs:   -
    */
    function _fnCaptureKeys() {
        if (_bKeyCapture == false) {
            _bKeyCapture = true;
            _fnRefreshFocusClass();
        }
    }


    /*
    * Function: _fnReleaseKeys
    * Purpose:  Stop capturing key events for this table
    * Returns:  -
    * Inputs:   -
    */
    function _fnReleaseKeys() {
        if (_bKeyCapture == true) {
            _bKeyCapture = false;
            _fnRefreshFocusClass();
        }
    }



    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    * Support functions
    */

    /*
    * Function: _fnCellFromCoords
    * Purpose:  Calulate the target TD cell from x and y coordinates
    * Returns:  node: - TD target
    * Inputs:   int:x - x coordinate
    *           int:y - y coordinate
    */
    function _fnCellFromCoords(x, y) {
        //return jQuery('tr:eq(' + y + ')>td:eq(' + x + ')', _nBody)[0];
        if (0 <= y && y < Bang_Viewport_N) {
            return (_tableId == BangDuLieuID_Slide) ? Bang_arrCell_Slide[y][x] : Bang_arrCell_Fixed[y][x];
        }
        return null;
    }


    /*
    * Function: _fnCoordsFromCell
    * Purpose:  Calculate the x and y position in a table from a TD cell
    * Returns:  array[2] int: [x, y]
    * Inputs:   node:n - TD cell of interest
    * Notes:    Not actually interested in this for DataTables since it might go out of date
    */
    function _fnCoordsFromCell(n) {
        return [
			jQuery('td', n.parentNode).index(n),
			jQuery('tr', n.parentNode.parentNode).index(n.parentNode)
		];
    }

    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    * Initialisation
    */

    /*
    * Function: _fnInit
    * Purpose:  Initialise the KeyTable
    * Returns:  -
    * Inputs:   object:oInit - optional - Initalisation object with the following parameters:
    *   array[2] int:focus - x and y coordinates of the initial target
    *     or
    *     node:focus - the node to set initial focus on
    *   node:table - the table to use, if not given, first table with class 'KeyTable' will be used
    *   string:focusClass - focusing class to give to table elements
    *           object:that - focus
    *   int:tabIndex - the tab index to give the hidden input element
    */
    function _fnInit(oInit, that) {
        /* Save scope */
        _that = that;

        /* Capture undefined initialisation and apply the defaults */
        if (typeof oInit == 'undefined') {
            oInit = {};
        }

        if (typeof oInit.focus == 'undefined') {
            oInit.focus = [0, 0];
        }

        if (typeof oInit.table == 'undefined') {
            oInit.table = jQuery('table.KeyTable')[0];
        }

        if (typeof oInit.focusClass != 'undefined') {
            _sFocusClass = oInit.focusClass;
        }

        if (typeof oInit.form == 'undefined') {
            oInit.form = false;
        }
        _bForm = oInit.form;

        /* Cache the tbody node of interest */
        _nBody = oInit.table.getElementsByTagName('tbody')[0];

        /* If the table is inside a form, then we need a hidden input box which can be used by the
        * browser to catch the browser tabbing for our table
        */
        if (_bForm) {
            var nDiv = document.createElement('div');
            _nInput = document.createElement('input');
            nDiv.style.height = "1px"; /* Opera requires a little something */
            nDiv.style.width = "0px";
            nDiv.style.overflow = "hidden";
            if (typeof oInit.tabIndex != 'undefined') {
                nDiv.tabIndex = oInit.tabIndex;
            }
            nDiv.appendChild(_nInput);
            oInit.table.parentNode.insertBefore(nDiv, oInit.table.nextSibling);

            jQuery(_nInput).focus(function () {
                /* See if we want to 'tab into' the table or out */
                //	console.log( 'focused' );
                if (!_bInputFocused) {
                    //console.log( 'focused action' );
                    _fnCaptureKeys();
                    _bInputFocused = false;
                    _fnSetFocus((typeof oInit.focus.nodeName != "undefined" ?
						oInit.focus :
						_fnCellFromCoords(oInit.focus[0], oInit.focus[1]))
					);
                    /* Need to interup the thread for this to work */
                    setTimeout(function () { _nInput.blur(); }, 0);
                }
            });
            _fnReleaseKeys();
        }
        else {
            /* Set the initial focus on the table */
            //            _fnSetFocus((typeof oInit.focus.nodeName != "undefined" ?
            //				oInit.focus :
            //				_fnCellFromCoords(oInit.focus[0], oInit.focus[1]))
            //			);
        }

        /*
        * Add event listeners
        * Well - I hate myself for doing this, but it would appear that key events in browsers are
        * a complete mess, particulay when you consider arrow keys, which of course are one of the
        * main areas of interest here. So basically for arrow keys, there is no keypress event in
        * Safari and IE, while there is in Firefox and Opera. But Firefox and Opera don't repeat the
        * keydown event for an arrow key. OUCH. See the following two articles for more:
        *   http://www.quirksmode.org/dom/events/keys.html
        *   https://lists.webkit.org/pipermail/webkit-dev/2007-December/002992.html
        *   http://unixpapa.com/js/key.html
        * PPK considers the IE / Safari method correct (good enough for me!) so we (urgh) detect
        * Mozilla and Opera and apply keypress for them, while everything else gets keydown. If
        * Mozilla or Opera change their implemention in future, this will need to be updated... 
        * although at the time of writing (14th March 2009) Minefield still uses the 3.0 behaviour.
        */
        if (jQuery.browser.mozilla || jQuery.browser.opera) {
            jQuery(document).bind("keypress", _fnKey);
        }
        else {
            jQuery(document).bind("keydown", _fnKey);
        }

        jQuery('td', _nBody).mousedown(_fnClick);
        jQuery('td', _nBody).dblclick(_fnDblClick);


        /* Loose table focus when click outside the table */
        jQuery(document).mousedown(function (e) {
            var nTarget = e.target;
            var bTableClick = false;
            while (nTarget) {
                if (nTarget == oInit.table) {
                    bTableClick = true;
                    break;
                }
                nTarget = nTarget.parentNode;
            }
            if (!bTableClick) {
                _fnBlur();
            }
        });
    }

    /* Initialise our new object */
    _fnInit(oInit, this);
}
