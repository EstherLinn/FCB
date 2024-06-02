<%@ page language="c#" %>

<%
    this.Response.StatusCode = 500;
    this.Response.TrySkipIisCustomErrors = true;
%>
<!DOCTYPE html>
<html>
<head>
    <title>An Error Occurred</title>
    <style>
        * { margin: 0; padding: 0; font-family: "Fira Code", monospace;}
        body { display: flex; flex-direction: column; justify-content: center; align-items: center; height: 100vh; background-color: #ecf0f1; }
        .container { text-align: center; margin: auto; padding: 4em;
            h1 { margin-top: 1rem; font-size: 35px; text-align: center;
                span { font-size: 60px; }
            }
            p { margin-top: 1rem; }
            p.info { margin-top: 4em; font-size: 12px;
                a { text-decoration: none; color: rgb(84, 84, 206); }
            }
        }
    </style>
</head>

<body>
    <div class="container">
        <h1>
            <span>500</span>
            <br />
            Internal server error
        </h1>
        <p>We are currently trying to fix the problem.</p>
    </div>
</body>

</html>