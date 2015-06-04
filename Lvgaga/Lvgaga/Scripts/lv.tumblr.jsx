var tumblrModels = [
  {
    MediaLargeUri: "https://qingyulustg.blob.core.windows.net/large/3b40a530-1b85-423e-b65c-6f14cf114d22.jpg",
    Text: "你也曾当过笨蛋，也曾试着当瞎子当聋子的去信任一个人，你也知道世界上最可悲的就是自我欺骗，但是人笨过傻过瞎过就够了，你更要懂得爱自己，而不是一直重蹈覆辙，还自以为多痴情。",
    CreateTime: "2015-06-02 16:33:41",
    tp: "",
    rk: "",
    comments: [
      {
        UserName: "bjutales@hotmail.com",
        CommentTime: "2015-06-02 16:33:41",
        Text: "一些评论"
      },
      {
        UserName: "bjutales@hotmail.com",
        CommentTime: "2015-06-02 16:33:41",
        Text: "一些评论"
      }
    ]
  },
  {
    MediaLargeUri: "https://qingyulustg.blob.core.windows.net/large/3b40a530-1b85-423e-b65c-6f14cf114d22.jpg",
    Text: "你也曾当过笨蛋，也曾试着当瞎子当聋子的去信任一个人，你也知道世界上最可悲的就是自我欺骗，但是人笨过傻过瞎过就够了，你更要懂得爱自己，而不是一直重蹈覆辙，还自以为多痴情。",
    CreateTime: "2015-06-02 16:33:41",
    tp: "",
    rk: "",
    comments: [
      {
        UserName: "bjutales@hotmail.com",
        CommentTime: "2015-06-02 16:33:41",
        Text: "一些评论"
      },
      {
        UserName: "bjutales@hotmail.com",
        CommentTime: "2015-06-02 16:33:41",
        Text: "一些评论"
      }
    ]
  }
];

React.render(
  <TumblrBoxList dataContext={tumblrModels} />,
  document.getElementById('div_tumblrs')
);