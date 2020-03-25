// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var firebaseConfig = {
    apiKey: "AIzaSyCZjQsTo0V6rxzS3pj7PEDm1Q1QdZkR0jk",
    authDomain: "cencalmushub.firebaseapp.com",
    databaseURL: "https://cencalmushub.firebaseio.com",
    projectId: "cencalmushub",
    storageBucket: "cencalmushub.appspot.com",
    messagingSenderId: "305255524575",
    appId: "1:305255524575:web:bedf9560548ba67be9a16a",
    measurementId: "G-CDEM6XQQTE"
};
// Initialize Firebase
firebase.initializeApp(firebaseConfig);
firebase.analytics();

firebase.auth().createUserWithEmailAndPassword(email, password).catch(function (error) {
    // Handle Errors here.
    var errorCode = error.code;
    var errorMessage = error.message;
    // ...
});

firebase.auth().signInWithEmailAndPassword(email, password).catch(function (error) {
    // Handle Errors here.
    var errorCode = error.code;
    var errorMessage = error.message;
    // ...
});