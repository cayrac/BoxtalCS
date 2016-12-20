$(document).ready(function(){
  $.expr[":"].onScreen = function(elem) {
    var $window = $(window)
    var viewport_top = $window.scrollTop()
    var viewport_height = $window.height()
    var viewport_bottom = viewport_top + viewport_height
    var $elem = $(elem)
    var top = $elem.offset().top
    var height = $elem.height()
    var bottom = top + height

    return (top >= viewport_top && top < viewport_bottom) ||
           (bottom > viewport_top && bottom <= viewport_bottom) ||
           (height > viewport_height && top <= viewport_top && bottom >= viewport_bottom)
  }

  var menu = [];
  var section = null;

  var email = "api" + "@" + "boxtal.com";
  $(".contact").html(email);
  $(".contact").attr("href", "mailto:" + email);

  $("#content h1, #content h2").each(function (i, el){
    if (el.nodeName == "H1")
    {
      if (section != null)
      {
        menu.push(section);
      }
      section = {
        el:el,
        childs:[]
      }
    }
    else {
      section.childs.push(el);
    }
  });
  if (section != null)
  {
    menu.push(section);
  }

  var menuNode = $("menu");
  var menuContent = "";
  for(var i in menu)
  {
    var section = menu[i];
    var id = i + "-0";
    $(section.el).attr("id", id);
    menuContent += "<h4><a href=\"#" + id + "\">" + section.el.innerHTML + "</a></h4>";
    menuContent += "<ul class=\"closed\">";
    for(var j in section.childs)
    {
      var child = section.childs[j];
      var id = i + "-" + (parseInt(j)+1);
      $(child).attr("id", id);
      menuContent += "<li><a href=\"#" + id + "\">" + child.innerHTML + "</a></li>";
    }
    menuContent += "</ul>";
  }
  $("menu").append(menuContent);

  var content = $("#content > *");
  var menu_links = $("menu a");
  var menu_uls = $("menu ul");
  function updateMenu()
  {
    var first = $(content.filter(":onScreen")[0]);

    if (first.length == 0)
    {
      first = content.filter(":last");
    }
    while (first.length > 0 && first.prop("tagName") != "H1" && first.prop("tagName") != "H2")
    {
      first = first.prev();
    }

    if (first.length == 0)
    {
      return;
    }

    menu_links.removeClass("highlight");
    menu_uls.addClass("closed");

    var id = first.attr("id");
    if (first.prop("tagName") == "H2")
    {
      menu_links.filter("[href='#" + id + "']").addClass("highlight");
      id = id.substr(0,2) + "0";
    }

    menu_links.filter("[href='#" + id + "']").addClass("highlight")
    .parent().next("ul").removeClass("closed");
  }

  $(document).scroll(updateMenu);
  updateMenu();
});
