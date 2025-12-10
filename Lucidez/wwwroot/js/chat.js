// chat.js — handles chat UI and real-time SignalR communication

(function () {

    const toggle = document.getElementById("chatToggle");
    const panel = document.getElementById("chatPanel");
    const msgBox = document.getElementById("chatMessages");
    const input = document.getElementById("chatInput");
    const nameInput = document.getElementById("chatName");
    const sendBtn = document.getElementById("chatSend");

    // Toggle open/close chat panel
    toggle.addEventListener("click", () => {
        panel.style.display = panel.style.display === "none" || panel.style.display === "" ? "block" : "none";
    });

    // Connect to SignalR Hub
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/chatHub")
        .withAutomaticReconnect()
        .build();

    connection.start().catch(err => console.error("SignalR Error:", err));

    // Receive messages
    connection.on("ReceiveMessage", (msg) => {
        const bubble = document.createElement("div");
        bubble.style.marginBottom = "10px";

        if (msg.sender === "admin") {
            bubble.innerHTML =
                `<div style="background:rgba(0,120,200,0.25); padding:8px; border-radius:10px; width:80%;">
                    <b>Support:</b> ${escapeHtml(msg.message)}
                </div>`;
        } else {
            bubble.innerHTML =
                `<div style="text-align:right;">
                    <div style="background:rgba(255,255,255,0.1); padding:8px; border-radius:10px; display:inline-block;">
                        <b>${escapeHtml(msg.sender)}:</b> ${escapeHtml(msg.message)}
                    </div>
                </div>`;
        }

        msgBox.appendChild(bubble);
        msgBox.scrollTop = msgBox.scrollHeight;
    });

    // Send message
    sendBtn.addEventListener("click", sendMessage);
    input.addEventListener("keypress", (e) => { if (e.key === "Enter") sendMessage(); });

    function sendMessage() {
        const sender = nameInput.value.trim() || "Guest";
        const text = input.value.trim();
        if (!text) return;

        connection.invoke("SendMessage", sender, text)
            .catch(err => console.error("Send Error:", err));

        input.value = "";
    }

    // Escape HTML to avoid XSS
    function escapeHtml(text) {
        return text.replace(/[&<>"']/g, function (m) {
            return ({
                "&": "&amp;",
                "<": "&lt;",
                ">": "&gt;",
                "\"": "&quot;",
                "'": "&#039;"
            })[m];
        });
    }

})();
