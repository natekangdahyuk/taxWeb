function niceEncodingString(msg) {
       var encodeKey    = "PrimeElectronicBookkeepingSystem";
       var suffleKey1   = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
       var suffleKey2   = "ZAnBglCost35GHIJ678wDpqrEWdefhijk2QSKL90XYabFNOPuvMVTmx4Ryz1Uc";
       var chars = msg.split("");
       var index;
       for (i = 0; i < chars.length; i++) {
             if ((index = suffleKey1.indexOf(chars[i])) != -1) {
                    chars[i] = suffleKey2.charAt(index);
             }
       }
       
       var key = stringToBytes(encodeKey);
       var bytes = stringToBytes(chars.join(""));
       var resultString = "";
       for (i = 0; i < bytes.length; i++) {
             bytes[i] = (bytes[i] ^ key[i % key.length]);
             if(bytes[i].toString(16).length < 2) {
                    resultString+="0" + bytes[i].toString(16);
             } else {
                    resultString+=bytes[i].toString(16);
             }      
       }
       return resultString.toUpperCase();
}

function stringToBytes ( str ) { 
       var ch, st, re = []; 
       for (var i = 0; i < str.length; i++ ) { 
             ch = str.charCodeAt(i);
             st = [];
             do { 
                    st.push( ch & 0xFF ); 
                    ch = ch >> 8;
             } while ( ch ); 
             re = re.concat( st.reverse() ); 
       } 
       return re; 
}
