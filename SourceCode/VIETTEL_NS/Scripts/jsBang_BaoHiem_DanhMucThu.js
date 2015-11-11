var BangDuLieu_CoCotDuyet = false;
var BangDuLieu_CoCotTongSo = true;

function BangDuLieu_onLoad() {
    if (document.getElementById('idCoCotDuyet') != null &&
            document.getElementById('idCoCotDuyet').value == "1") {
        BangDuLieu_CoCotDuyet = true;
    }
    if (document.getElementById('idCoCotTongSo') != null &&
            document.getElementById('idCoCotTongSo').value == "0") {
        BangDuLieu_CoCotTongSo = false;
    }
}

function BangDuLieu_onCellAfterEdit(h, c) {
    BangDuLieu_CapNhatThayDoi(h); 
}
function BangDuLieu_CapNhatThayDoi(h) {
    var c = 0;
    var cMax = Bang_nC;
    for (c = 0; c < cMax; c++) {                
            Bang_arrThayDoi[h][c] = true;        
    }
}
function BangDuLieu_CapNhapLaiHangCha(h, c) {
    if (Bang_arrType[c] == 1) {
        if (BangDuLieu_CoCotDuyet && c >= Bang_arrMaCot.length - 2) {
            return;
        }
        BangDuLieu_TinhOTongSo(h);
        var csCha = h, GiaTri;
        while (Bang_arrCSCha[csCha] >= 0) {
            csCha = Bang_arrCSCha[csCha];
            GiaTri = BangDuLieu_TinhTongHangCon(csCha, c);
            Bang_GanGiaTriThatChoO(csCha, c, GiaTri);
            BangDuLieu_TinhOTongSo(csCha);
        }
    }
}

function BangDuLieu_TinhOTongSo(h) {
    if (BangDuLieu_CoCotTongSo) {
        var GT = 0, c, cTongSo = Bang_arrCSMaCot["rTongSo"];
        var cMax = Bang_nC;
        for (c = Bang_nC_Fixed+1; c < cMax-1; c++) {
            if (Bang_arrMaCot[c].startsWith("r")) {
                GT = GT + Bang_arrGiaTri[h][c];
            }
        }
        Bang_GanGiaTriThatChoO(h, cTongSo, GT, 1);
    }
}

function BangDuLieu_TinhTongHangCon(csCha, c) {
    var h, vR = 0;
    for (h = 0; h < Bang_arrCSCha.length; h++) {
        if (csCha == Bang_arrCSCha[h]) {
            vR += parseFloat(Bang_arrGiaTri[h][c]);
        }
    }
    return vR;
}

var BangDuLieu_CheckAll_value = false;
function BangDuLieu_CheckAll() {
    BangDuLieu_CheckAll_value = !BangDuLieu_CheckAll_value;
    var value = "0";
    if (BangDuLieu_CheckAll_value) {
        value = "1";
    }
    else {
        value = "0";
    }
    var h, c = Bang_arrMaCot.length - 2;
    for (h = 0; h < Bang_arrMaHang.length; h++) {
        if (Bang_arrLaHangCha[h] == false) {
            Bang_GanGiaTriThatChoO(h, c, value);
        }
    }
}

function BangDuLieu_onKeypress_Delete(h, c) {
    if (h != null) {
        if (Bang_arrLaHangCha[h] == false) {
            Bang_XoaHang(h);
            //            //Xoa ca vao bang BangDuLieu_arrChiSoNhom
            //            BangDuLieu_arrChiSoNhom.splice(h, 1);
            //            //Xoa ca vao bang BangDuLieu_arrMaMucLucNganSach
            //            BangDuLieu_arrMaMucLucNganSach.splice(h, 1);
            //            if (h < Bang_nH) {
            Bang_keys.fnSetFocus(h, c);
        }
        else if (Bang_nH > 0) {

        }
    }
}
    