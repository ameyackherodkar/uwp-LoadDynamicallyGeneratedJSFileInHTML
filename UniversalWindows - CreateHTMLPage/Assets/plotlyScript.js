var trace1 = {
    x: [1, 2, 3, 4,5],
    y: [4, 6, 3, 5,6],
    type: 'scatter'
};

var data = [trace1];

var layout = {
    title: 'Graph Title'
};

Plotly.newPlot(graphDiv, data, layout);