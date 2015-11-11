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
    BangDuLieu_CapNhapHang(h, c);
    BangDuLieu_CapNhapHangTong();
}
function BangDuLieu_CapNhapLaiHangCha(h, c) {
    if (Bang_arrMaCot[c].startsWith("rTien")) {
        if (Bang_arrType[c] == 1) {
            if (BangDuLieu_CoCotDuyet && c >= Bang_arrMaCot.length - 2) {
                return;
            }
            var csCha = h, GiaTri;
            for (i = 0; i <= Bang_arrCSCha.length; i = i + 1) {
            }
            while (Bang_arrCSCha[csCha] >= 0) {
                csCha = Bang_arrCSCha[csCha];
                GiaTri = BangDuLieu_TinhTongHangCon(csCha, c);
                Bang_GanGiaTriThatChoO(csCha, c, GiaTri);
            }
        }
    } 
}
function BangDuLieu_TinhChiPhiBH(h, c) {
    if (Bang_arrMaCot[c].startsWith("rTien")) {
        if (Bang_arrType[c] == 1) {
            var strTTCha = Bang_arrGiaTri[Bang_arrCSCha[h]][0];
            var MaCha = strTTCha.split('.').join('');
            if (parseInt(MaCha) > 100) {
                var TyLe = getHeSoBH(MaCha);
                var ChiPhi = Bang_arrGiaTri[Bang_arrCSCha[h]][c];
                var MaGan = parseInt(MaCha) + 10;
                var strTTGan = "";
                for (i = 0; i < MaGan.toString().length; i = i + 1) {
                    var spliter = ".";
                    if (i == 0) spliter = "";
                    strTTGan += spliter + MaGan.toString()[i];
                }
                for (ih = 0; ih < Bang_nH; ih = ih + 1) {
                    var strTT = Bang_arrGiaTri[ih][0];
                    if (strTT == strTTGan) {
                        var ChiPhiGan = ChiPhi * TyLe;
                        Bang_GanGiaTriThatChoO(ih, c, ChiPhiGan);
                        BangDuLieu_onCellAfterEdit(ih, c);
                        break;
                    }
                }
            }
        }
    }
}
function BangDuLieu_TuDongTinhCacMucTyLe(h, c) {
    if (Bang_arrMaCot[c].startsWith("rTien")) {
        if (Bang_arrType[c] == 1) {
            var strTTCha = Bang_arrGiaTri[Bang_arrCSCha[h]][0];
            var ChiPhi = Bang_arrGiaTri[Bang_arrCSCha[h]][c];
            if (strTTCha == "2.1.1") {
                BangDuLieu_GanGiaTriTheoMa("3.1.1", c, ChiPhi * getTyLe("311"));
                BangDuLieu_GanGiaTriTheoMa("4.1.1", c, ChiPhi * getTyLe("411"));
                BangDuLieu_GanGiaTriTheoMa("5.1.1", c, ChiPhi * getTyLe("511"));

                BangDuLieu_GanGiaTriTheoMa("2.2.1", c, ChiPhi * getHeSoBH("211"));
                BangDuLieu_GanGiaTriTheoMa("3.2.1", c, ChiPhi * getTyLe("311") * getHeSoBH("311"));
                BangDuLieu_GanGiaTriTheoMa("4.2.1", c, ChiPhi * getTyLe("411") * getHeSoBH("411"));
                BangDuLieu_GanGiaTriTheoMa("5.2.1", c, ChiPhi * getTyLe("511") * getHeSoBH("511"));
            }
            if (strTTCha == "2.1.2") {
                BangDuLieu_GanGiaTriTheoMa("3.1.2", c, ChiPhi * getTyLe("312"));
                BangDuLieu_GanGiaTriTheoMa("4.1.2", c, ChiPhi * getTyLe("412"));
                BangDuLieu_GanGiaTriTheoMa("5.1.2", c, ChiPhi * getTyLe("512"));

                BangDuLieu_GanGiaTriTheoMa("2.2.2", c, ChiPhi * getHeSoBH("212"));
                BangDuLieu_GanGiaTriTheoMa("3.2.2", c, ChiPhi * getTyLe("312") * getHeSoBH("312"));
                BangDuLieu_GanGiaTriTheoMa("4.2.2", c, ChiPhi * getTyLe("412") * getHeSoBH("412"));
                BangDuLieu_GanGiaTriTheoMa("5.2.2", c, ChiPhi * getTyLe("512") * getHeSoBH("512"));
            }
        }
    }
}
function BangDuLieu_GanGiaTriTheoMa(MaGan, c, GiaTri) {
    for (ih = 0; ih < Bang_nH; ih = ih + 1) {
        var strTT = Bang_arrGiaTri[ih][0];
        if (strTT == MaGan) {
            Bang_GanGiaTriThatChoO(ih, c, GiaTri);
            BangDuLieu_onCellAfterEdit(ih, c);
            break;
        }
    }
}
function BangDuLieu_LayGiaTriTheoMa(MaGan, c) {
    for (ih = 0; ih < Bang_nH; ih = ih + 1) {
        var strTT = Bang_arrGiaTri[ih][0];
        if (strTT == MaGan) {
            return Bang_arrGiaTri[ih][c];
        }
    }
}
function BangDuLieu_CapNhapHang(h, c) {
    if (c == 3 || c == 6 || c == 9 || c == 12) {
    // cot so luong 
        var SoLuong = Bang_arrGiaTri[h][c];
        var DonGia = Bang_arrGiaTri[h][c + 1];
        var Tien = SoLuong * DonGia;
        Bang_GanGiaTriThatChoO(h, c + 2, Tien, 1);
        BangDuLieu_CapNhapLaiHangCha(h, c + 2);
        BangDuLieu_TuDongTinhCacMucTyLe(h, c + 2);
    } else if (c == 4 || c == 7 || c == 10 || c == 13) {
        // cot don gia
        var SoLuong = Bang_arrGiaTri[h][c - 1];
        var DonGia = Bang_arrGiaTri[h][c];
        var Tien = SoLuong * DonGia;
        Bang_GanGiaTriThatChoO(h, c + 1, Tien, 1);
        BangDuLieu_CapNhapLaiHangCha(h, c + 1);
        BangDuLieu_TuDongTinhCacMucTyLe(h, c + 1);
    } else {
        BangDuLieu_CapNhapLaiHangCha(h, c);
        BangDuLieu_TuDongTinhCacMucTyLe(h, c);
    }
}
function BangDuLieu_CapNhapCotSoSanh(h) {
    var TienDeNghi = Bang_arrGiaTri[h][14];
    var TienThucHien = Bang_arrGiaTri[h][5];
    if (TienThucHien != 0) {
        var SoSanh = TienDeNghi / TienThucHien * 100;
        Bang_GanGiaTriThatChoO(h, 15, SoSanh, 1);
    }
}
function BangDuLieu_TinhOTongSo(h) {
    if (BangDuLieu_CoCotTongSo) {
        var GT = 0, c, cTongSo = Bang_arrCSMaCot["rTongSo"];
        var cMax = Bang_nC;
        for (c = Bang_nC_Fixed; c < cMax; c++) {
            if (Bang_arrMaCot[c].startsWith("rTien")) {
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
function getNSQP(c) {
    var gt = 0;
    var _h = 0;
    for (_h = 0; _h < Bang_nH; _h = _h + 1) {
        if (parseFloat(Bang_arrGiaTri[_h][Bang_nC - 1]) == 1) {
            gt += parseFloat(Bang_arrGiaTri[_h][c - 1]) * parseFloat(Bang_arrGiaTri[_h][c - 2]);
        }
    }
    return gt;
}
function BangDuLieu_CapNhapHangTong() {
    for (c = 0; c < Bang_nC; c = c + 1) {
        if (Bang_arrMaCot[c].startsWith("rTien")) {
            var rTien15 = 0, rTien47 = 0, rTien43 = 0;
            var Tong = 0, rTien6 = 0, rTien7 = 0, rTien8 = 0, rTien9 = 0;
            for (h = 0; h < Bang_nH; h = h + 1) {
                var strTT = Bang_arrGiaTri[h][0];
                if (!(strTT.indexOf(".") > 0) && strTT != "") {
                    if (parseInt(strTT) < 6) {
                        Tong += Bang_arrGiaTri[h][c];
                    } if (parseInt(strTT) == 6) {
                        rTien6 = Tong;
                        Bang_GanGiaTriThatChoO(h, c, rTien6);
                    } else if (parseInt(strTT) == 7) {
                        rTien7 = getLoiNhuan() * (rTien6 - rTien15 - rTien43 - rTien47);
                        Bang_GanGiaTriThatChoO(h, c, rTien7);
                    } else if (parseInt(strTT) == 8) {
                        rTien8 = getThueGTGT() * (rTien6 + rTien7);
                        Bang_GanGiaTriThatChoO(h, c, rTien8);
                    } else if (parseInt(strTT) == 9) {
                        rTien9 = rTien6 + rTien7 + rTien8;
                        Bang_GanGiaTriThatChoO(h, c, rTien9);
                    } else if (parseInt(strTT) == 10) {
                        Bang_GanGiaTriThatChoO(h, c, rTien9 - getNSQP(c));
                    }
                } else {
                    if (strTT == "1.5") rTien15 = Bang_arrGiaTri[h][c];
                    if (strTT == "4.7") rTien47 = Bang_arrGiaTri[h][c];
                    if (strTT == "4.3") rTien43 = Bang_arrGiaTri[h][c];
                }
                BangDuLieu_CapNhapCotSoSanh(h);
            }
        }
    }
}