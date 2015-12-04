// create audio element

// play song

// onAnimationFrame, capture FFT

// create json object

// download object

    var timestamps = [];
	var data = [];

    // create the audio context (chrome only for now)
    // create the audio context (chrome only for now)
    if (! window.AudioContext) {
        if (! window.webkitAudioContext) {
            alert('no audiocontext found');
        }
        window.AudioContext = window.webkitAudioContext;
    }
    var context = new AudioContext();
    var audioBuffer;
    var sourceNode;
    var analyser;
    var javascriptNode;

    // load the sound
    setupAudioNodes();
    loadSound("chaos.wav");


    function setupAudioNodes() {

        // setup a javascript node
        javascriptNode = context.createScriptProcessor(2048, 1, 1);
        // connect to destination, else it isn't called
        javascriptNode.connect(context.destination);


        // setup a analyzer
        analyser = context.createAnalyser();
        analyser.smoothingTimeConstant = 1/60;
        analyser.fftSize = 2048;

        // create a buffer source node
        sourceNode = context.createBufferSource();
        sourceNode.connect(analyser);

        sourceNode.connect(context.destination);
    }

    // load the specified sound
    function loadSound(url) {
        console.log(url);
        var request = new XMLHttpRequest();
        request.open('GET', url, true);
        request.responseType = 'arraybuffer';

        // When loaded decode the data
        request.onload = function() {

            // decode the data
            context.decodeAudioData(request.response, function(buffer) {
                // when the audio is decoded play the sound
                analyser.connect(javascriptNode);
                playSound(buffer);
            }, onError);
        };
        request.send();
    }

    var startTime;

    function playSound(buffer) {
        sourceNode.buffer = buffer;
        sourceNode.start(0);
        sourceNode.onended = onComplete;
        startTime = Date.now();
    }

    // log if an error occurs
    function onError(e) {
        console.log(e);
    }

    // when the javascript node is called
    // we use information from the analyzer node
    // to draw the volume
    javascriptNode.onaudioprocess = function() {

        // get the average for the first channel
        var array8 =  new Uint8Array(analyser.frequencyBinCount);
        analyser.getByteFrequencyData(array8);
        var array = array8.join(",").split(",").map(function(a){return parseInt(a);});
        if (!startTime) return;
        timestamps.push(Date.now() - startTime);
        data.push(array);

    };

    function onComplete(){
    	// var a = document.createElement("a");
        // a.innerHTML = "Get Data";
    	// document.body.appendChild(a);
    	// a.download = "fftdata.json";
        var indexed = {};
        data = data.map(function(e, i){
            return {
                time: timestamps[i],
                data: data[i]
            };
        });
        console.log(JSON.stringify(data, null, "\t"));

        // console.log(JSON.stringify(indexed));
    	// a.href = "data:application/json;," + JSON.stringify(indexed);

        var pre = document.createElement("pre");
        pre.innerHTML = JSON.stringify(data, null, "\t");

        document.body.appendChild(pre);

    }

