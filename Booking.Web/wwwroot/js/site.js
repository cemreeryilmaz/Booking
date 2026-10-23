// =========================================================
//  StayFinder - Arayüz etkileşimleri
//  (Tüm davranışlar ilgili eleman varsa çalışır; yoksa sessizce atlanır.)
// =========================================================

(function () {
    "use strict";

    // ---------- 1) Misafir / oda sayaçları (ana sayfa) ----------
    document.querySelectorAll(".counter").forEach(function (counter) {
        var input = counter.querySelector(".counter-value");
        if (!input) return;

        var min = parseInt(counter.dataset.min, 10);
        var max = parseInt(counter.dataset.max, 10);

        counter.addEventListener("click", function (e) {
            var btn = e.target.closest(".counter-btn");
            if (!btn) return;

            var value = parseInt(input.value, 10) || 0;
            value = btn.dataset.action === "plus"
                ? Math.min(value + 1, max)
                : Math.max(value - 1, min);
            input.value = value;

            // Çocuk sayısı değişince yaş alanını göster/gizle
            if (counter.id === "childCounter") {
                var wrapper = document.getElementById("childAgeWrapper");
                if (wrapper) wrapper.classList.toggle("d-none", value === 0);
            }
        });
    });

    // ---------- 2) Favori (kalp) butonları (liste + detay) ----------
    document.querySelectorAll(".fav-btn").forEach(function (btn) {
        btn.addEventListener("click", function () {
            var icon = btn.querySelector("i");
            var active = btn.classList.toggle("is-active");
            if (icon) icon.className = active ? "bi bi-heart-fill" : "bi bi-heart";
            btn.setAttribute("aria-label", active ? "Favorilerden çıkar" : "Favorilere ekle");
        });
    });

    // ---------- 3) Fotoğraf slider'ı (detay sayfası) ----------
    var track = document.getElementById("photoTrack");
    if (track) {
        document.querySelectorAll(".slider-btn").forEach(function (btn) {
            btn.addEventListener("click", function () {
                var item = track.querySelector(".photo-item");
                var gap = parseFloat(getComputedStyle(track).columnGap) || 0;
                var step = item ? item.offsetWidth + gap : track.clientWidth;
                track.scrollBy({ left: step * parseInt(btn.dataset.dir, 10), behavior: "smooth" });
            });
        });
    }

    // ---------- 4) Arama formu "loading" durumu ----------
    // Arama başlatıldığında kullanıcıya yükleniyor geri bildirimi gösterilir.
    document.querySelectorAll("form[data-search-form]").forEach(function (form) {
        form.addEventListener("submit", function () {
            var overlay = document.getElementById("sfLoading");
            if (overlay) overlay.classList.add("is-active");
        });
    });

    // ---------- 5) Basit "toast" bildirimi ----------
    function showToast(message) {
        var toast = document.createElement("div");
        toast.className = "sf-toast";
        toast.setAttribute("role", "status");
        toast.innerHTML = '<i class="bi bi-check-circle"></i><span></span>';
        toast.querySelector("span").textContent = message;
        document.body.appendChild(toast);
        // Giriş animasyonu için bir sonraki frame'de sınıf ekle
        requestAnimationFrame(function () { toast.classList.add("is-visible"); });
        setTimeout(function () {
            toast.classList.remove("is-visible");
            setTimeout(function () { toast.remove(); }, 300);
        }, 2600);
    }

    // ---------- 6) Paylaş butonu (detay sayfası) ----------
    document.querySelectorAll("[data-share]").forEach(function (btn) {
        btn.addEventListener("click", async function () {
            var shareData = { title: document.title, url: window.location.href };
            if (navigator.share) {
                try { await navigator.share(shareData); } catch (e) { /* kullanıcı iptal etti */ }
            } else if (navigator.clipboard && navigator.clipboard.writeText) {
                try {
                    await navigator.clipboard.writeText(shareData.url);
                    showToast("Bağlantı panoya kopyalandı");
                } catch (e) {
                    showToast("Bağlantı kopyalanamadı");
                }
            }
        });
    });

    // ---------- 7) Rezervasyon (demo) akışı ----------
    var reservationModalEl = document.getElementById("reservationModal");
    if (reservationModalEl && window.bootstrap) {
        var reservationModal = bootstrap.Modal.getOrCreateInstance(reservationModalEl);
        var summaryView = document.getElementById("reservationSummary");
        var successView = document.getElementById("reservationSuccess");
        var confirmBtn = document.getElementById("reservationConfirm");

        function openReservation(roomName, roomPrice) {
            // Özet/başarı görünümlerini sıfırla
            if (summaryView) summaryView.classList.remove("d-none");
            if (successView) successView.classList.add("d-none");
            if (confirmBtn) confirmBtn.classList.remove("d-none");

            if (roomName) {
                var roomEl = reservationModalEl.querySelector("[data-res-room]");
                if (roomEl) roomEl.textContent = roomName;
            }
            // Oda fiyatı yoksa ("-" veya boş) otelin başlangıç fiyatı (varsayılan) kalsın.
            if (roomPrice && roomPrice !== "-") {
                var priceEl = reservationModalEl.querySelector("[data-res-price]");
                if (priceEl) priceEl.textContent = roomPrice;
            }
            reservationModal.show();
        }

        // "Oda Seç" (özet kartı): odalar varsa oraya kaydır, yoksa modalı aç
        document.querySelectorAll("[data-book]").forEach(function (btn) {
            btn.addEventListener("click", function (e) {
                e.preventDefault();
                var rooms = document.getElementById("rooms");
                if (rooms && rooms.querySelector(".room-card")) {
                    rooms.scrollIntoView({ behavior: "smooth", block: "start" });
                } else {
                    openReservation(null, null);
                }
            });
        });

        // "Odayı Seç" (her oda kartı): modalı o odanın bilgileriyle aç
        document.querySelectorAll(".btn-select-room").forEach(function (btn) {
            btn.addEventListener("click", function () {
                openReservation(btn.dataset.roomName, btn.dataset.roomPrice);
            });
        });

        // Onayla: başarı görünümüne geç
        if (confirmBtn) {
            confirmBtn.addEventListener("click", function () {
                if (summaryView) summaryView.classList.add("d-none");
                if (successView) successView.classList.remove("d-none");
                confirmBtn.classList.add("d-none");
            });
        }
    }
})();
