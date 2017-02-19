'use strict';

var gulp = require('gulp'),
    watch = require('gulp-watch'),
    prefixer = require('gulp-autoprefixer'),
    uglify = require('gulp-uglify'),
    sass = require('gulp-sass'),
    sourcemaps = require('gulp-sourcemaps'),
    rigger = require('gulp-rigger'),
    cssmin = require('gulp-minify-css'),
    imagemin = require('gulp-imagemin'),
    pngquant = require('imagemin-pngquant'),
    rimraf = require('rimraf'),
    browserSync = require("browser-sync"),
    reload = browserSync.reload;

var path = {
    build: { //Тут мы укажем куда складывать готовые после сборки файлы
        html: '../Themes/karelian-profile/html/',
        js: '../Themes/karelian-profile/js/',
        css: '../Themes/karelian-profile/css/',
        img: '../Themes/karelian-profile/img/',
        fonts: '../Themes/karelian-profile/css/partials/fonts/'
    },
    src: { //Пути откуда брать исходники
        html: 'src/*.html', //Синтаксис src/*.html говорит gulp что мы хотим взять все файлы с расширением .html
        js: 'src/js/main.js',//В стилях и скриптах нам понадобятся только main файлы
        style: 'src/css/main.scss',
        img: 'src/img/**/*.{jpg,jpeg,png,gif}', //Синтаксис img/**/*.* означает - взять все файлы всех расширений из папки и из вложенных каталогов
        fonts: 'src/fonts/**/*.*'
    },
    watch: { //Тут мы укажем, за изменением каких файлов мы хотим наблюдать
        html: 'src/**/*.html',
        js: 'src/js/**/*.js',
        css: 'src/css/**/*.scss',
        img: 'src/img/**/*.*',
        fonts: 'src/fonts/**/*.*'
    },
    clean: '../Themes/karelian-profile'
};

/*var config = {
    server: {
        baseDir: "../Themes/karelian-profile/html"
    },
    tunnel: true,
    host: 'localhost',
    port: 9000,
    logPrefix: "Frontend_Devil"
};*/

gulp.task('html:karelian-profile', function () {
    gulp.src(path.src.html) //Выберем файлы по нужному пути
        .pipe(rigger()) //Прогоним через rigger
        .pipe(gulp.dest("../Themes/karelian-profile/html")) //Выплюнем их в папку build
        .pipe(reload({stream: true})); //И перезагрузим наш сервер для обновлений
});

gulp.task('js:karelian-profile', function () {
    gulp.src(path.src.js) //Найдем наш main файл
        .pipe(rigger()) //Прогоним через rigger
        .pipe(sourcemaps.init()) //Инициализируем sourcemap
        .pipe(uglify()) //Сожмем наш js
        .pipe(sourcemaps.write()) //Пропишем карты
        .pipe(gulp.dest("../Themes/karelian-profile/js")) //Выплюнем готовый файл в build
        .pipe(reload({stream: true})); //И перезагрузим сервер
});

gulp.task('css:karelian-profile', function () {
    gulp.src("src/css/main.scss") //Выберем наш main.scss
        .pipe(sourcemaps.init()) //То же самое что и с js
        .pipe(sass()) //Скомпилируем
        .pipe(prefixer()) //Добавим вендорные префиксы
        .pipe(cssmin()) //Сожмем
        .pipe(sourcemaps.write())
        .pipe(gulp.dest("../Themes/karelian-profile/css")) //И в build
        .pipe(reload({stream: true}));
});

gulp.task('image:karelian-profile', function () {
    gulp.src("src/img/**/*.*") //Выберем наши картинки
        .pipe(imagemin({ //Сожмем их
            progressive: true,
            svgoPlugins: [{removeViewBox: false}],
            use: [pngquant()],
            interlaced: true
        }))
        .pipe(gulp.dest("../Themes/karelian-profile/img")) //И бросим в build
        .pipe(reload({stream: true}));
});

gulp.task('fonts:karelian-profile', function() {
    gulp.src("src/fonts/*")
        .pipe(gulp.dest("../Themes/karelian-profile/css/partials/fonts"))
});

gulp.task('karelian-profile', [
    'html:karelian-profile',
    'js:karelian-profile',
    'css:karelian-profile',
    'fonts:karelian-profile',
    'image:karelian-profile'
]);


gulp.task('watch', function(){
    watch([path.watch.html], function(event, cb) {
        gulp.start('html:karelian-profile');
    });
    watch([path.watch.css], function(event, cb) {
        gulp.start('css:karelian-profile');
    });
    watch([path.watch.js], function(event, cb) {
        gulp.start('js:karelian-profile');
    });
    watch([path.watch.img], function(event, cb) {
        gulp.start('image:karelian-profile');
    });
    watch([path.watch.fonts], function(event, cb) {
        gulp.start('fonts:karelian-profile');
    });
});

/*gulp.task('webserver', function () {
    browserSync(config);
});*/

gulp.task('clean', function (cb) {
    rimraf(path.clean, cb);
});

gulp.task('default', ['karelian-profile'/*, 'webserver'*/, 'watch']);





