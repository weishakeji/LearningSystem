$ready(function () {
    function cache(version) {
        var db_version = version ? version : Number($api.storage('cache_db_version'));
        db_version = isNaN(db_version) ? 1 : db_version;
        var request = window.indexedDB.open("cache_weishakeji", ++db_version);
        var db = null;
        var tablename = "Question_ForCourse6";
        request.onsuccess = function (event) {
            db = event.target.result;
            var ver = db.version;
            $api.storage('cache_db_version', ver);
            console.log('数据库打开成功,version:' + ver);
            //遍历存储对象
            var stores = db.objectStoreNames;
            for (let i = 0; i < stores.length; i++) {
                console.log(stores[i]);
            }

            if (!db.objectStoreNames.contains(tablename)) {
                console.log('不存在');
                //cache(ver+1);
            }
            //addData(db, tablename);
            /*
            var t = getDataByKey(db, tablename, '1001', function (value) {
                console.log('回调取值：' + value);
            });
            */

        }
        request.onupgradeneeded = function (event) {
            db = event.target.result;
            var objectStore;
            if (!db.objectStoreNames.contains(tablename)) {
                objectStore = db.createObjectStore(tablename, { keyPath: "id" });
            }
        }

    }
    //cache();
    var md5 = $api.md5('1');
    console.log(md5);
    //数据库名称
    window.dbname = "apicache_test";
    //数据库版本号，最新版本号加一
    window.dbversion = function (version) {
        var ver_name = 'cache_db_version';
        if (version == null) {
            var db_version = Number($api.storage(ver_name));
            db_version = isNaN(db_version) ? 1 : db_version;
            return db_version;
        } else {
            $api.storage(ver_name, version);
            return version;
        }
    };
    /*
    createStore("Question_ForCourse6").then(function (db) {
        console.log(db);
    }).catch(function (err) {
        //alert(err);
        console.error(err);
        createStore(err.value);
    });*/
    /*
        var t = getbykey("Question_ForCourse8", '1002');
        t.then(function (req) {
            console.log(req);
        }).catch(function (err) {
            //alert(err);
            console.error(err);
            //createStore(err.value);
        });*/


    addData("Question_ForCourse9", {
        id: '1005', name: "大王", age: 24
    });
    getall("Question_ForCourse8").then(function (d) {
        console.log('getall:');
        console.log(d);
    }).catch(function (err) {
        console.error(err);

    });
    /*deldata("Question_ForCourse8","1001").then(function (d) {
      console.log('删除成功:');
      console.log(d);
  }).catch(function (err) {
      console.error(err);

  });*/
    //创建存储空间
    function createStore(storename) {
        return new Promise(function (resolve, reject) {
            var request = window.indexedDB.open(window.dbname, window.dbversion());
            request.onupgradeneeded = function (event) {
                db = event.target.result;
                var objectStore;
                if (!db.objectStoreNames.contains(storename)) {
                    objectStore = db.createObjectStore(storename, { keyPath: "id" });
                }
                resolve(db);
            };
            request.onsuccess = function (event) {
                db = event.target.result;
                window.dbversion(db.version);
            };
        });
    }
    //添加数据(storeName:存储空间名称，类似表; data：要添加的数据)    
    function addData(storeName, data, version) {
        version = version ? version : window.dbversion();
        new Promise(function (resolve, reject) {
            var request = window.indexedDB.open(window.dbname, version);
            request.onerror = function (event) {
                console.log('request.onerror');
            };
            request.onblocked = function (event) {
                console.log('request.onblocked');
            };
            request.onupgradeneeded = function (event) {
                var db = event.target.result;
                var objectStore;
                if (!db.objectStoreNames.contains(storeName)) {
                    objectStore = db.createObjectStore(storeName, { keyPath: "id" });
                }
                window.dbversion(db.version + 1);
            }
            request.onsuccess = function (event) {
                var db = event.target.result;
                if (!db.objectStoreNames.contains(storeName)) {
                    var err = error(0, '存储空间"' + storeName + '"不存在', db, storeName);
                    reject(err);
                    db.close();
                } else {
                    resolve(db);
                }
            };
        }).then(function (db) {           
            var transaction = db.transaction([storeName], 'readwrite');
            var store = transaction.objectStore(storeName);
            store.put(data);
            db.close();
        }).catch(function (err) {
            if (err.state == 0) {
                err.db.close();
                addData(storeName, data, err.db.version + 1);
            }
        });

    }
    //通过key值获取value
    function getbykey(storeName, key) {
        return new Promise(function (resolve, reject) {
            var request = window.indexedDB.open(window.dbname, window.dbversion());
            var db = null;
            request.onsuccess = function (event) {
                db = event.target.result;
                if (!db.objectStoreNames.contains(storeName)) {
                    var err = error(0, '存储空间"' + storeName + '"不存在', db, storeName);
                    reject(err);
                    return;
                }               
                var transaction = db.transaction(storeName, 'readwrite');
                var store = transaction.objectStore(storeName);
                var storeget = store.get(key);
                var value;
                storeget.onsuccess = function (e) {
                    value = e.target.result;
                };
                transaction.oncomplete = function (event) {
                    if (!value) {
                        var err = error(0, '数据项"' + storeName + ':' + key + '"不存在', db, {
                            store: storeName,
                            key: key
                        });
                        reject(err);
                        return;
                    }
                    resolve(value);

                }
            };
        });
    }
    //获取某个存储空间下的所有对象
    function getall(storeName) {
        return new Promise(function (resolve, reject) {
            function _getall(storeName, callback) {
                var request = window.indexedDB.open(window.dbname, window.dbversion());
                var db = null;
                request.onsuccess = function (event) {
                    db = event.target.result;
                    if (!db.objectStoreNames.contains(storeName)) {
                        var err = error(0, '存储空间"' + storeName + '"不存在', db, storeName);
                        reject(err);
                        return;
                    }
                    //window.dbversion(db.version);
                    var transaction = db.transaction(storeName);
                    var store = transaction.objectStore(storeName);
                    var storeget = store.openCursor();
                    let data = [];
                    storeget.onsuccess = function (e) {
                        let result = e.target.result;
                        if (result && result !== null) {
                            data.push(result.value);
                            result.continue();
                        } else {
                            if (callback) callback(data);
                        }
                    };
                };
            }
            _getall(storeName, function (data) {
                resolve(data);
            })
        });
    }
    //删除数据
    function deldata(storeName, key) {
        return new Promise(function (resolve, reject) {
            var request = window.indexedDB.open(window.dbname, window.dbversion());
            var db = null;
            request.onsuccess = function (event) {
                db = event.target.result;
                if (!db.objectStoreNames.contains(storeName)) {
                    var err = error(0, '存储空间"' + storeName + '"不存在', db, storeName);
                    reject(err);
                    return;
                }
                //window.dbversion(db.version);
                var store = db.transaction(storeName, 'readwrite').objectStore(storeName);
                var trans = store.delete(key);                
                trans.onsuccess = function (e) {
                    resolve(key);
                };
                trans.onerror = function (event) {
                    var err = error(1, '数据项"' + storeName + ':' + key + '"删除失败', db, {
                        store: storeName,
                        key: key
                    });
                    reject(err);
                }
            };
        });
    }
    //错误信息的返回值
    //state:0不存在，-1为其它
    function error(state, message, database, value) {
        return {
            'state': state,
            'message': message,
            'db': database,
            'value': value
        }
    }
});