// minimal particles + reveal animations for home
(function () {
    // particles canvas
    const canvas = document.getElementById('particles');
    if (canvas) {
        const ctx = canvas.getContext('2d');
        let w = canvas.width = innerWidth;
        let h = canvas.height = innerHeight;
        const orbs = [];
        const ORB_COUNT = Math.max(40, Math.floor((w * h) / 90000));
        function rand(min, max) { return Math.random() * (max - min) + min; }
        class Orb {
            constructor() { this.reset(true); }
            reset(init = false) {
                this.x = rand(0, w); this.y = rand(0, h);
                this.vx = rand(-0.25, 0.25); this.vy = rand(-0.15, 0.15);
                this.r = rand(0.6, 2.6); this.alpha = rand(0.06, 0.28); this.hue = rand(180, 210);
                if (!init) { this.x = Math.random() > 0.5 ? -10 : w + 10; this.y = rand(0, h); }
            }
            step() {
                this.x += this.vx; this.y += this.vy;
                if (this.x < -20 || this.x > w + 20 || this.y < -20 || this.y > h + 20) this.reset();
            }
            draw() {
                const grd = ctx.createRadialGradient(this.x, this.y, 0, this.x, this.y, this.r * 18);
                grd.addColorStop(0, `hsla(${this.hue},100%,60%,${this.alpha})`);
                grd.addColorStop(0.4, `hsla(${this.hue},100%,50%,${this.alpha * 0.45})`);
                grd.addColorStop(1, `hsla(${this.hue},100%,45%,0)`);
                ctx.fillStyle = grd;
                ctx.fillRect(this.x - this.r * 18, this.y - this.r * 18, this.r * 36, this.r * 36);
            }
        }
        function setup() {
            w = canvas.width = innerWidth; h = canvas.height = innerHeight;
            orbs.length = 0; for (let i = 0; i < ORB_COUNT; i++) orbs.push(new Orb());
        }
        function loop() {
            ctx.clearRect(0, 0, w, h);
            for (const o of orbs) { o.step(); o.draw(); }
            requestAnimationFrame(loop);
        }
        addEventListener('resize', setup);
        setup(); loop();
    }

    // reveal elements
    document.addEventListener('DOMContentLoaded', function () {
        const els = document.querySelectorAll('.reveal');
        const obs = new IntersectionObserver((entries) => {
            entries.forEach(e => {
                if (e.isIntersecting) { e.target.classList.add('show'); obs.unobserve(e.target); }
            });
        }, { threshold: 0.18 });
        els.forEach(el => obs.observe(el));
    });

})();
// mobile menu toggle (append to lucidez-home.js)
(function () {
    const mobileBtn = document.getElementById('mobileMenuBtn');
    const mobileMenu = document.getElementById('mobileMenu');

    if (mobileBtn && mobileMenu) {
        mobileBtn.addEventListener('click', (e) => {
            e.stopPropagation();
            mobileMenu.style.display = mobileMenu.style.display === 'block' ? 'none' : 'block';
            // small animation
            mobileMenu.style.opacity = mobileMenu.style.display === 'block' ? '1' : '0';
        });

        // close when clicking outside
        document.addEventListener('click', (e) => {
            if (!mobileMenu.contains(e.target) && e.target !== mobileBtn) {
                mobileMenu.style.display = 'none';
            }
        });

        // close on link click (for smooth UX)
        mobileMenu.querySelectorAll('a').forEach(a => a.addEventListener('click', () => {
            mobileMenu.style.display = 'none';
        }));
    }
})();

(() => {
    const launcher = document.getElementById("chatLauncher");
    const box = document.getElementById("chatBox");
    const closeBtn = document.getElementById("chatClose");
    const sendBtn = document.getElementById("chatSend");
    const input = document.getElementById("chatInput");
    const nameInput = document.getElementById("chatName");
    const messages = document.getElementById("chatMessages");

    launcher.onclick = () => box.style.display = "flex";
    closeBtn.onclick = () => box.style.display = "none";

    sendBtn.onclick = send;
    input.addEventListener("keydown", e => e.key === "Enter" && send());

    function send() {
        const text = input.value.trim();
    if (!text) return;

    const name = nameInput.value || "You";

    const msg = document.createElement("div");
    msg.className = "chat-bubble user";
    msg.innerHTML = `<strong>${name}:</strong><br>${text}`;

        messages.appendChild(msg);
        input.value = "";
        messages.scrollTop = messages.scrollHeight;

        // SignalR hook here (already implemented on your side)
    }
})();

