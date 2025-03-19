//document.getElementById("fileInput").addEventListener("change", function () {
//    const file = this.files[0];

//    if (file && file.type === "application/pdf") {
//        const fileURL = URL.createObjectURL(file);

//        // Show the PDF in the iframe
//        const pdfViewer = document.getElementById("pdfViewer");
//        pdfViewer.src = fileURL;

//        // Reveal the preview container
//        const previewContainer = document.getElementById("previewContainer");
//        previewContainer.style.display = "block";
//    } else {
//        alert("Please upload a valid PDF file.");
//    }
//});


//document.getElementById("fileInput").addEventListener("change", async function () {
//    const file = this.files[0];
//    const formData = new FormData();
//    formData.append("file", file);

//    // Send file to backend
//    const response = await fetch("/Lessons/ExtractPdfContent", {
//        method: "POST",
//        body: formData,
//    });

//    const data = await response.json();

//    if (data.success) {
//        console.log("PDF Content:", data.content);

//        // Display the content below the PDF viewer
//        const contentDisplay = document.createElement("div");
//        contentDisplay.innerHTML = `<h3>Extracted Content:</h3><p>${data.content}</p>`;
//        document.getElementById("previewContainer").appendChild(contentDisplay);

//        // (Optional) Send content to AI
//        const aiResponse = await fetch("/Lessons/AnalyzeWithChatGPT", {
//            method: "POST",
//            headers: { "Content-Type": "application/json" },
//            body: JSON.stringify({ content: data.content }),
//        });

//        const aiData = await aiResponse.json();
//        console.log("AI Analysis:", aiData);
//        contentDisplay.innerHTML += `<h3>AI Analysis:</h3><p>${aiData}</p>`;
//    } else {
//        alert("Failed to extract content.");
//    }
//});

document.getElementById("uploadForm").addEventListener("submit", async function (e) {
    e.preventDefault();

    const fileInput = document.getElementById("pdfFile");
    const file = fileInput.files[0];

    if (!file) {
        alert("Please select a PDF file.");
        return;
    }

    const formData = new FormData();
    formData.append("pdfFile", file);

    try {
        const response = await fetch("/Pdf/UploadPdfAndGenerateQA", {
            method: "POST",
            body: formData
        });

        const data = await response.json();

        if (data.success) {
            const pdfViewer = document.getElementById("pdfViewer");
            const aiOutput = document.getElementById("aiOutput");

            // Show PDF in viewer
            const pdfUrl = URL.createObjectURL(file);
            pdfViewer.src = pdfUrl;
            pdfViewer.style.display = "block";

            // Show AI-generated content
            aiOutput.innerHTML = `<h5>Generated Questions & Answers:</h5><p>${data.content.replace(/\n/g, "<br>")}</p>`;
            aiOutput.style.display = "block";
        } else {
            alert(data.message);
        }
    } catch (error) {
        console.error("Error:", error);
        alert("An error occurred while uploading the file.");
    }
});
